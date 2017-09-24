namespace ScorchEngine.Server
{
    [System.Serializable]
    public class PlayerState
    {
        public int Id;
        public bool IsReady;
        public float AngleHorizontal;
        public float AngleVertical;
        public float Force;
        public bool IsActive;
        public bool IsValid;

        public float PositionX;
        public float PositionY;
        public float PositionZ;


        public PlayerState()
        {
            
        }
        
        public override string ToString()
        {
            return string.Format("{0},{1},({2},{3}),({4},{5},{6}),{7},{8},{9}",
                Id,
                IsReady,
                AngleHorizontal,
                AngleVertical,
                PositionX,
                PositionY,
                PositionZ,
                Force,
                IsActive,
                IsValid);
        }
    }
}