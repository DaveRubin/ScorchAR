namespace ScorchEngine.Server
{
    public class PlayerState
    {
        public int Id { get; set; }
        public bool IsReady { get; set; }
        public float AngleHorizontal { get; set; }
        public float AngleVertical { get; set; }
        public float Force { get; set; }

        public override string ToString()
        {
            return string.Format("{0},{1},({2},{3}),{4}",
                Id,
                IsReady,
                AngleHorizontal,
                AngleVertical,
                Force);
        }
    }
}