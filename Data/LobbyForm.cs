using System.ComponentModel.DataAnnotations;

public class LobbyForm
{
    [Required(ErrorMessage = "Lobby name is required")]
    [StringLength(32, ErrorMessage = "Lobby name must not exceed 32 characters")]
    public string LobbyName { get; set; } 

    [Required(ErrorMessage = "Player name is required")]
    [StringLength(32, ErrorMessage = "Player name must not exceed 32 characters")]
    public string PlayerName { get; set; } 
}