//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookingBus.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Abonnement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Abonnement()
        {
            this.Effectuers = new HashSet<Effectuer>();
        }

        public int id_abonnement { get; set; }
        [Display(Name="Date de début")]
        public System.DateTime date_debut { get; set; }
        [Display(Name = "Date de fin")]
        public System.DateTime date_fin { get; set; }
        public int id_navette { get; set; }
        public float prix { get; set; }
        [Display(Name = "Nom abonnement")]
        public string nom_abonnement { get; set; }
        public string image { get; set; }
        public Nullable<int> id_societe { get; set; }
    
        public virtual Societe Societe1 { get; set; }
        public virtual Navette Navette { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Effectuer> Effectuers { get; set; }
    }
}
