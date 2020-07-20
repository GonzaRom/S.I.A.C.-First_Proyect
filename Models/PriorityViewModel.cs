namespace S.I.A.C.Models
{
    public class PriorityViewModel
    {
        public PriorityViewModel()
        {
        }

        public PriorityViewModel(int keyPriority, string valuePriority)
        {
            this.keyPriority = keyPriority;
            this.valuePriority = valuePriority;
        }

        public int keyPriority { get; set; }
        public string valuePriority { get; set; }
    }
}