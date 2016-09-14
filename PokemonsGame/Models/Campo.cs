using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PokemonsGame.Models
{
    public class Campo
    {
        [Key]
        public int Id { get; set; }


        public int MyPokemonId { get; set; }

        public int EnemyPokemonId { get; set; }

        [ForeignKey("MyPokemonId")]
        public virtual Pokemon MyPokemon { get; set; }

        [ForeignKey("EnemyPokemonId")]
        public virtual Pokemon EnemyPokemon { get; set; }
    }
}