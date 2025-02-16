using System.ComponentModel.DataAnnotations;

namespace SkiServerBackend.Models
{
    public class Dienstleistung
    {
        [Key]
        public int DienstleistungId { get; set; } // Stelle sicher, dass es `DienstleistungId` heißt
        public string Name { get; set; } = string.Empty;
       
    }

}
