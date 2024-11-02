using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Typesense.Setup;

public record TypesenseOptions
{
    [Required]
    public string Url { get; set; }
    
    [Required]
    public string ApiKey { get; set; }    
}