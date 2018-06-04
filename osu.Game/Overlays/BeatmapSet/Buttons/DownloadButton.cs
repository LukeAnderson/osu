﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using OpenTK;

namespace osu.Game.Overlays.BeatmapSet.Buttons
{
    public class DownloadButton : HeaderButton
    {
        public DownloadButton(string title, string subtitle, BeatmapSetInfo set, bool noVideo = false)
        {
            Width = 120;

            BeatmapSetDownloader downloader;
            Add(new Container
            {
                Depth = -1,
                RelativeSizeAxes = Axes.Both,
                Padding = new MarginPadding { Horizontal = 10 },
                Children = new Drawable[]
                {
                    downloader = new BeatmapSetDownloader(set, noVideo),
                    new FillFlowContainer
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Children = new[]
                        {
                            new OsuSpriteText
                            {
                                Text = title,
                                TextSize = 13,
                                Font = @"Exo2.0-Bold",
                            },
                            new OsuSpriteText
                            {
                                Text = subtitle,
                                TextSize = 11,
                                Font = @"Exo2.0-Bold",
                            },
                        },
                    },
                    new SpriteIcon
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Icon = FontAwesome.fa_download,
                        Size = new Vector2(16),
                        Margin = new MarginPadding { Right = 5 },
                    },
                },
            });

            Action = downloader.Download;

            downloader.Downloaded.ValueChanged += d =>
            {
                if (d)
                    this.FadeOut(200);
                else
                    this.FadeIn(200);
            };
        }
    }
}
