namespace ScorchEngine.Models {
    public class PlayerInfo {
        public string name;
        public string id;
        override public string ToString() {
            return string.Format("(Player #{0} - {1})",id,name);
        }
    }
}