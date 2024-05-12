namespace PrzykladowyKolosB.DTOs;

public record ReturnBookDTO(int idBook, string title, List<AutorDTO> authors);