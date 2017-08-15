namespace ScorchEngine.Models {
    public class PlayerInfo {
        public string Name { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return $@"Name:{Name}
Id:{Id}";
        }
    }
}