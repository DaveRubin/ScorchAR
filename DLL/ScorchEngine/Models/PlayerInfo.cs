namespace ScorchEngine.Models {

    public class PlayerInfo {

        public string Id { get; set; }
        public string Name { get; set; }
        

        public override string ToString()
        {
            return $@"Name:{Name}
Id:{Id}";
        }
    }
}