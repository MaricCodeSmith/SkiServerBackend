using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkiServerBackend.Data;
using SkiServerBackend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ServiceAuftragController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ServiceAuftragController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ✅ 1️⃣ Liste aller offenen Serviceaufträge abrufen (KEINE Authentifikation erforderlich)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceAuftrag>>> GetOffeneAuftraege()
    {
        var offeneAuftraege = await _context.ServiceAuftraege
            .Where(a => a.StatusID == 1) // 1 = "Offen"
            .Include(a => a.Kunde) // Kunde-Daten mitladen
            .Include(a => a.Dienstleistung) // Dienstleistung-Daten mitladen
            .ToListAsync();

        return Ok(offeneAuftraege);
    }

    // ✅ 2️⃣ Status eines Auftrags ändern (AUTHENTIFIKATION ERFORDERLICH)
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusUpdateRequest request)
    {
        var auftrag = await _context.ServiceAuftraege.FindAsync(id);

        if (auftrag == null)
        {
            return NotFound(new { message = "Auftrag nicht gefunden." });
        }

        if (!new[] { "Offen", "InArbeit", "Abgeschlossen" }.Contains(request.Status))
        {
            return BadRequest(new { message = "Ungültiger Status." });
        }

        auftrag.StatusID = request.Status switch
        {
            "Offen" => 1,
            "InArbeit" => 2,
            "Abgeschlossen" => 3,
            _ => auftrag.StatusID // Falls keine Änderung notwendig
        };

        await _context.SaveChangesAsync();
        return Ok(new { message = "Status erfolgreich aktualisiert." });
    }

    
    [HttpPut("{id}/status")]
    [Authorize] // Nur eingeloggte Mitarbeiter dürfen den Status ändern
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
    {
        var auftrag = await _context.ServiceAuftraege.FindAsync(id);
        if (auftrag == null)
            return NotFound(new { message = "Serviceauftrag nicht gefunden." });

        auftrag.StatusID = request.StatusID;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Status aktualisiert", auftrag });
    }

    public class UpdateStatusRequest
    {
        public int StatusID { get; set; } // 1 = Offen, 2 = InArbeit, 3 = Abgeschlossen
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteServiceAuftrag(int id)
    {
        var auftrag = await _context.ServiceAuftraege.FindAsync(id);
        if (auftrag == null)
            return NotFound(new { message = "Serviceauftrag nicht gefunden." });

        _context.ServiceAuftraege.Remove(auftrag);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Serviceauftrag wurde gelöscht." });
    }
   
    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<ServiceAuftrag>>> FilterServiceAuftraege(
    [FromQuery] int? status,
    [FromQuery] int? prioritaet)
    {
        var query = _context.ServiceAuftraege.AsQueryable();

        if (status.HasValue)
            query = query.Where(s => s.StatusID == status.Value);

        if (prioritaet.HasValue)
            query = query.Where(s => s.Priorität == prioritaet.Value);

        var result = await query.ToListAsync();
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateMitarbeiter(int id, [FromBody] MitarbeiterUpdateRequest request)
    {
        var mitarbeiter = await _context.Mitarbeiter.FindAsync(id);
        if (mitarbeiter == null)
            return NotFound(new { message = "Mitarbeiter nicht gefunden." });

        mitarbeiter.Name = request.Name;
        mitarbeiter.Email = request.Email;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Mitarbeiter aktualisiert", mitarbeiter });
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteMitarbeiter(int id)
    {
        var mitarbeiter = await _context.Mitarbeiter.FindAsync(id);
        if (mitarbeiter == null)
            return NotFound(new { message = "Mitarbeiter nicht gefunden." });

        _context.Mitarbeiter.Remove(mitarbeiter);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Mitarbeiter gelöscht." });
    }

    public class MitarbeiterUpdateRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }


}

// 📌 DTOs für API-Anfragen
public class StatusUpdateRequest
{
    public string Status { get; set; }
}
