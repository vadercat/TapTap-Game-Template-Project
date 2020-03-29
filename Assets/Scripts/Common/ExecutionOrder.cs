namespace TapTap
{
    public static class ExecutionOrder
    {
        public const int MinOrder = int.MinValue;

        public const int Injector = MinOrder + 1;

        // Custom.

        public const int GameLogic = -2;

        public const int LevelGenerator = -1;
    }
}
