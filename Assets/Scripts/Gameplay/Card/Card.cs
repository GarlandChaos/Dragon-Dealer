namespace Game.Gameplay
{
    public enum Element
    {
        NONE,
        GRASS,
        FIRE,
        WATER
    }

    public readonly struct Card
    {
        public readonly Element element;
        public readonly int value;

        public Card(Element element, int value)
        {
            this.element = element;
            this.value = value;
        }
    }
}