using System.Diagnostics.CodeAnalysis;

namespace TP1_ASP.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }
    public string? MensajeError {get; set;}

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public ErrorViewModel (string? MensajeError) {
        this.MensajeError = MensajeError;
    }
}
