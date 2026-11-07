using MonoMod.RuntimeDetour;
using UnityEngine.Video;
using TeamCherry.Cinematics;

namespace CustomKnight.Skin.Cinematics
{
    // Taken from HKMirror
    // https://github.com/TheMulhima/HKMirror

    internal static class CinematicHelper
    {
        public static class Delegates
        {
            public delegate VideoClip get_EmbeddedVideoClip_AfterArgs(Params_get_EmbeddedVideoClip args, VideoClip ret);

            public delegate void get_EmbeddedVideoClip_BeforeArgs(Params_get_EmbeddedVideoClip args);

            public delegate VideoClip get_EmbeddedVideoClip_WithArgs(Func<CinematicVideoReference, VideoClip> orig,
                CinematicVideoReference self);

            public sealed class Params_get_EmbeddedVideoClip
            {
#pragma warning disable CS0649 // Field 'CinematicHelper.Delegates.Params_get_EmbeddedVideoClip.self' is never assigned to, and will always have its default value null
                public CinematicVideoReference self;
#pragma warning restore CS0649 // Field 'CinematicHelper.Delegates.Params_get_EmbeddedVideoClip.self' is never assigned to, and will always have its default value null
            }
        }
        private static readonly List<KeyValuePair<Delegates.get_EmbeddedVideoClip_WithArgs, Hook>> videoClipHooks = new();

        public static event Delegates.get_EmbeddedVideoClip_WithArgs get_EmbeddedVideoClip
        {
            add
            {
                if (value == null) return;
                var hook = new Hook(
                    ReflectionHelper.GetMethodInfo(typeof(CinematicVideoReference), "get_EmbeddedVideoClip"), value);
                videoClipHooks.Add(new(value, hook));
            }
            remove
            {
                // Match event semantics when the same handler is added more than once.
                int index = videoClipHooks.FindLastIndex(entry => entry.Key == value);
                if (index < 0) return;
                videoClipHooks[index].Value.Dispose();
                videoClipHooks.RemoveAt(index);
            }
        }
    }

    internal class CinematicSequenceR
    {
        internal T GetField<T>(string name)
        {
            return ReflectionHelper.GetField<CinematicSequence, T>(this.orig, name);
        }
        internal void SetField<T>(string name, T value)
        {
            ReflectionHelper.SetField<CinematicSequence, T>(this.orig, name, value);
        }
        internal CinematicSequence orig;
        internal CinematicSequenceR(CinematicSequence orig)
        {
            this.orig = orig;
        }

        public CinematicVideoReference videoReference
        {
            get => GetField<CinematicVideoReference>("videoReference");
            set => SetField("videoReference", value);
        }

        public int framesSinceBegan
        {
            get => GetField<int>("framesSinceBegan");
            set => SetField("framesSinceBegan", value);
        }
    }
}
