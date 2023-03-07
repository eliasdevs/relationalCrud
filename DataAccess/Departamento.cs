using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestJoin1.DataAccess;

public class Departamento
{
   
    [Key]
    [Required(ErrorMessage = "Por favor selecciona un Departamento")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Por favor agrega un nombre")]
    public string? Nombre { get; set; }
}
