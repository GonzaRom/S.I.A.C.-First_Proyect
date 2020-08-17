namespace S.I.A.C.Models.DomainModels
{
    public class rolOperations
    {
        public int id { get; set; }
        public int? idRol { get; set; }
        public int? idOperations { get; set; }

        public virtual operations operations { get; set; }
        public virtual rol rol { get; set; }
    }
}