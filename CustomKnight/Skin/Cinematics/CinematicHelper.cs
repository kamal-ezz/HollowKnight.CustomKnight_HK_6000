using TeamCherry.Cinematics;

namespace CustomKnight.Skin.Cinematics
{
    // Taken from HKMirror
    // https://github.com/TheMulhima/HKMirror

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
