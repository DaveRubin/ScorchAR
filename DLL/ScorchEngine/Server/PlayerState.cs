namespace ScorchEngine.Server
{
    public class PlayerState
    {
        public int ID;
        public bool IsReady;
        public float AngleHorizontal;
        public float AngleVertical;
        public float Force;

        public string ToString()
        {
            return string.Format("{0},{1},({2},{3}),{4}", ID,
                IsReady,
                AngleHorizontal,
                AngleVertical,
                Force);
        }
    }
}