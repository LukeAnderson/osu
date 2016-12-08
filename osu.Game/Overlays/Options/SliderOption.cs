﻿using System;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Transformations;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input;

namespace osu.Game.Overlays.Options
{
    public class SliderOption<T> : FlowContainer where T : struct,
        IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
    {
        private SliderBar<T> slider;
        private SpriteText text;
    
        public string LabelText
        {
            get { return text.Text; }
            set
            {
                text.Text = value;
                text.Alpha = string.IsNullOrEmpty(value) ? 0 : 1;
            }
        }
        
        public BindableNumber<T> Bindable
        {
            get { return slider.Bindable; }
            set { slider.Bindable = value; }
        }

        public SliderOption()
        {
            Direction = FlowDirection.VerticalOnly;
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Children = new Drawable[]
            {
                text = new SpriteText { Alpha = 0 },
                slider = new OsuSliderBar<T>
                {
                    Margin = new MarginPadding { Top = 5 },
                    RelativeSizeAxes = Axes.X,
                }
            };
        }

        private class OsuSliderBar<U> : SliderBar<U> where U : struct,
            IComparable, IFormattable, IConvertible, IComparable<U>, IEquatable<U>
        {
            private AudioSample sample;
            private double lastSample;
            
            private Container nub;
            private Box leftBox, rightBox;
            public OsuSliderBar()
            {
                Height = 22;
                Children = new Drawable[]
                {
                    leftBox = new Box
                    {
                        Height = 2,
                        RelativeSizeAxes = Axes.None,
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Colour = new Color4(255, 102, 170, 255),
                    },
                    rightBox = new Box
                    {
                        Height = 2,
                        RelativeSizeAxes = Axes.None,
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Colour = new Color4(255, 102, 170, 255),
                        Alpha = 0.5f,
                    },
                    nub = new Container
                    {
                        Width = Height,
                        Height = Height,
                        CornerRadius = Height / 2,
                        Origin = Anchor.TopCentre,
                        AutoSizeAxes = Axes.None,
                        RelativeSizeAxes = Axes.None,
                        Masking = true,
                        BorderColour = new Color4(255, 102, 170, 255),
                        BorderThickness = 3,
                        Children = new[]
                        {
                            new Box { Colour = Color4.Transparent, RelativeSizeAxes = Axes.Both }
                        }
                    },
                };
            }

            [BackgroundDependencyLoader]
            private void load(AudioManager audio)
            {
                sample = audio.Sample.Get(@"Sliderbar/sliderbar");
            }

            private void playSample()
            {
                if (Clock == null)
                    return;
                if (Clock.CurrentTime - lastSample > 50)
                {
                    lastSample = Clock.CurrentTime;
                    sample.Frequency.Value = 1 + NormalizedValue * 0.2f;
                    sample.Play();
                }
            }
            
            protected override bool OnKeyDown(InputState state, KeyDownEventArgs args)
            {
                if (args.Key == Key.Left || args.Key == Key.Right)
                    playSample();
                return base.OnKeyDown(state, args);
            }
            
            protected override bool OnClick(InputState state)
            {
                playSample();
                return base.OnClick(state);
            }
            
            protected override bool OnDrag(InputState state)
            {
                playSample();
                return base.OnDrag(state);
            }

            protected override void UpdateValue(float value)
            {
                nub.MoveToX(DrawWidth * value, 300, EasingTypes.OutQuint);
                leftBox.ScaleTo(new Vector2(
                        MathHelper.Clamp(DrawWidth * value - nub.Width / 2 + 2, 0, DrawWidth),
                    1), 300, EasingTypes.OutQuint);
                rightBox.ScaleTo(new Vector2(
                        MathHelper.Clamp(DrawWidth * (1 - value) - nub.Width / 2 + 2, 0, DrawWidth),
                    1), 300, EasingTypes.OutQuint);
            }
        }
    }
}