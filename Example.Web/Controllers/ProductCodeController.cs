using Example.Web.Data;
using Example.Web.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Example.Web.Controllers;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public class ProductCodeController(AppDbContext db) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? search, CancellationToken cancellationToken)
    {
        search = search?.Trim();
        ViewData["Search"] = search;
        var query = db.ProductCodeItems.AsNoTracking();
        if (!string.IsNullOrEmpty(search))
            query = query.Where(x => EF.Functions.Collate(x.Code, "Latin1_General_100_CI_AS").Contains(search));
        return View(await query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id).ToListAsync(cancellationToken));
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(string? code, CancellationToken cancellationToken)
    {
        var enteredCode = code?.Trim().ToUpperInvariant() ?? string.Empty;
        var payload = enteredCode.Replace("-", string.Empty);
        var formattedCode = string.Join("-", Enumerable.Range(0, 6).Select(index => payload.Substring(index * 5, 5)));
        if (await db.ProductCodeItems.AnyAsync(x => x.Code == formattedCode, cancellationToken))
        {
            TempData["Error"] = "รหัสสินค้านี้มีอยู่แล้ว";
            return RedirectToAction(nameof(Index));
        }

        db.ProductCodeItems.Add(new ProductCodeItem { Code = formattedCode, CreatedAt = DateTime.UtcNow });
        try
        {
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (
            exception.InnerException is SqlException { Number: 2601 or 2627 })
        {
            TempData["Error"] = "รหัสสินค้านี้มีอยู่แล้ว";
            return RedirectToAction(nameof(Index));
        }

        TempData["Message"] = "เพิ่มรหัสสินค้าเรียบร้อยแล้ว";
        return RedirectToAction(nameof(Index));
    }

    private static bool IsUppercaseLetterOrDigit(char character) =>
        character is >= 'A' and <= 'Z' or >= '0' and <= '9';

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, string? search, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest();
        var item = await db.ProductCodeItems.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (item is null) return NotFound();
        db.ProductCodeItems.Remove(item);
        await db.SaveChangesAsync(cancellationToken);
        TempData["Message"] = "ลบรหัสสินค้าเรียบร้อยแล้ว";
        return RedirectToAction(nameof(Index), new { search });
    }
}

