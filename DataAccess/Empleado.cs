using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestJoin1.DataAccess;

public class Empleado
{
    [Required]
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Por favor agrega un nombre")]
    public string? Nombre { get; set; }
    [Required(ErrorMessage ="Por favor selecciona un valor")]
    
    
    public Departamento Departamento { get; set; }=new Departamento();
}
