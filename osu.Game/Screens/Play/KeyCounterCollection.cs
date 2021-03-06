﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Input;

namespace osu.Game.Screens.Play
{
    public class KeyCounterCollection : FillFlowContainer<KeyCounter>
    {
        public KeyCounterCollection()
        {
            Direction = FillDirection.Horizontal;
            AutoSizeAxes = Axes.Both;
        }

        public override void Add(KeyCounter key)
        {
            base.Add(key);
            key.IsCounting = IsCounting;
            key.FadeTime = FadeTime;
            key.KeyDownTextColor = KeyDownTextColor;
            key.KeyUpTextColor = KeyUpTextColor;
        }

        public void ResetCount()
        {
            foreach (var counter in Children)
                counter.ResetCount();
        }

        public override bool Contains(Vector2 screenSpacePos) => true;

        //further: change default values here and in KeyCounter if needed, instead of passing them in every constructor
        private bool isCounting;
        public bool IsCounting
        {
            get { return isCounting; }
            set
            {
                if (value != isCounting)
                {
                    isCounting = value;
                    foreach (var child in Children)
                        child.IsCounting = value;
                }
            }
        }

        private int fadeTime;
        public int FadeTime
        {
            get { return fadeTime; }
            set
            {
                if (value != fadeTime)
                {
                    fadeTime = value;
                    foreach (var child in Children)
                        child.FadeTime = value;
                }
            }
        }

        private Color4 keyDownTextColor = Color4.DarkGray;
        public Color4 KeyDownTextColor
        {
            get { return keyDownTextColor; }
            set
            {
                if (value != keyDownTextColor)
                {
                    keyDownTextColor = value;
                    foreach (var child in Children)
                        child.KeyDownTextColor = value;
                }
            }
        }

        private Color4 keyUpTextColor = Color4.White;
        public Color4 KeyUpTextColor
        {
            get { return keyUpTextColor; }
            set
            {
                if (value != keyUpTextColor)
                {
                    keyUpTextColor = value;
                    foreach (var child in Children)
                        child.KeyUpTextColor = value;
                }
            }
        }

        public override bool HandleInput => receptor?.IsAlive != true;

        private Receptor receptor;

        public Receptor GetReceptor()
        {
            return receptor ?? (receptor = new Receptor(this));
        }

        public class Receptor : Drawable
        {
            private KeyCounterCollection target;

            public Receptor(KeyCounterCollection target)
            {
                RelativeSizeAxes = Axes.Both;
                this.target = target;
            }

            public override bool Contains(Vector2 screenSpacePos) => true;

            public override bool HandleInput => true;

            protected override bool OnKeyDown(InputState state, KeyDownEventArgs args) => target.Children.Any(c => c.TriggerKeyDown(state, args));

            protected override bool OnKeyUp(InputState state, KeyUpEventArgs args) => target.Children.Any(c => c.TriggerKeyUp(state, args));

            protected override bool OnMouseDown(InputState state, MouseDownEventArgs args) => target.Children.Any(c => c.TriggerMouseDown(state, args));

            protected override bool OnMouseUp(InputState state, MouseUpEventArgs args) => target.Children.Any(c => c.TriggerMouseUp(state, args));
        }
    }
}
