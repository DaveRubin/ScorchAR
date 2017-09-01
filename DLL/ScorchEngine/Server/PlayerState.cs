namespace ScorchEngine.Server
{
    public class PlayerState
    {
        public int Id { get; set; }
        public bool IsReady { get; set; }
        public float AngleHorizontal { get; set; }
        public float AngleVertical { get; set; }
        public float Force { get; set; }
        public bool IsActive { get; set; }

        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }

        public override string ToString()
        {
            return string.Format("{0},{1},({2},{3}),({4},{5},{6}),{7},{8}",
                Id,
                IsReady,
                AngleHorizontal,
                AngleVertical,
                PositionX,
                PositionY,
                PositionZ,
                Force,
                IsActive);
        }
    }
}