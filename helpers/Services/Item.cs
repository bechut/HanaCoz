using System.Threading.Tasks;
using HanaCoz.Helpers.Models;
using Microsoft.Data.Sqlite;

namespace HanaCoz.Helpers.Services;

public class ItemService(SqliteConnection connection)
{
  public async Task Init()
  {
    var inserts = new[]
    {
      """
      INSERT INTO item (id, code, name, type, region_x, region_y, region_w, region_h, description)
      SELECT 1, '0101', 'Weekend Wanderer', 'outfit', 34,44,28,20,
             'A comfortable and timeless combination of faded denim jeans, a soft cotton t-shirt, and canvas sneakers. Perfect for running errands or grabbing coffee.'
      WHERE NOT EXISTS (SELECT 1 FROM item WHERE id = 1);
      """,
      """
      INSERT INTO item (id, code, name, type, region_x, region_y, region_w, region_h, description)
      SELECT 2, '0102', 'Midnight Gala Set', 'outfit', 34,44,28,20,
             'A sleek, tailored velvet blazer, matching trousers, a crisp white button-down shirt, and leather dress shoes. Worn for black-tie events or high-stakes meetings.'
      WHERE NOT EXISTS (SELECT 1 FROM item WHERE id = 2);
      """,
      """
      INSERT INTO item (id, code, name, type, region_x, region_y, region_w, region_h, description)
      SELECT 3, '0103', 'Trail Explorer Rig', 'outfit', 34,44,28,20,
             'Durable, quick-drying cargo pants, a technical moisture-wicking synthetic jacket, waterproof hiking boots, and a wide-brimmed hat. Essential for wilderness trekking.'
      WHERE NOT EXISTS (SELECT 1 FROM item WHERE id = 3);
      """,
      """
      INSERT INTO item (id, code, name, type, region_x, region_y, region_w, region_h, description)
      SELECT 4, '0104', 'Cozy Homebody', 'outfit', 34,44,28,20,
             'Oversized knit sweatpants, a thick, plush hooded sweatshirt, and fuzzy slipper socks. Designed purely for maximum relaxation and indoor comfort.'
      WHERE NOT EXISTS (SELECT 1 FROM item WHERE id = 4);
      """,
      """
      INSERT INTO item (id, code, name, type, region_x, region_y, region_w, region_h, description)
      SELECT 5, '0105', 'Athleisure Commuter', 'outfit', 34,44,28,20,
             'Black leggings or joggers, a lightweight performance vest, running shoes, and a slim backpack. Blends athletic gear with street style for active daily wear.'
      WHERE NOT EXISTS (SELECT 1 FROM item WHERE id = 5);
      """,
      """
      INSERT INTO item (id, code, name, type, region_x, region_y, region_w, region_h, description)
      SELECT 6, '0101', 'Wind-Tousled Layers', 'hairstyle', 578,84,28,18,
             'Carefree layers that flow naturally, as if brushed by a gentle breeze. Perfect for adventurers who prefer effortless charm.'
      WHERE NOT EXISTS (SELECT 1 FROM item WHERE id = 6);
      """,
      """
      INSERT INTO item (id, code, name, type, region_x, region_y, region_w, region_h, description)
      SELECT 7, '0102', 'Crimson Streak Ponytail', 'hairstyle', 578,84,28,18,
             'A high ponytail bound with a bold crimson streak that radiates confidence and rebellious energy.'
      WHERE NOT EXISTS (SELECT 1 FROM item WHERE id = 7);
      """,
      """
      INSERT INTO item (id, code, name, type, region_x, region_y, region_w, region_h, description)
      SELECT 8, '0103', 'Midnight Fade', 'hairstyle', 578,84,28,18,
             'A sleek modern fade blending deep black tones with subtle midnight-blue highlights — sharp, mysterious, and stylish.'
      WHERE NOT EXISTS (SELECT 1 FROM item WHERE id = 8);
      """,
      """
      INSERT INTO item (id, code, name, type, region_x, region_y, region_w, region_h, description)
      SELECT 9, '0104', 'Golden Spiral Curls', 'hairstyle', 578,84,28,18,
             'Soft golden curls that bounce with every step, glowing under sunlight. A timeless style that exudes warmth and joy.'
      WHERE NOT EXISTS (SELECT 1 FROM item WHERE id = 9);
      """,
      """
      INSERT INTO item (id, code, name, type, region_x, region_y, region_w, region_h, description)
      SELECT 10, '0105', 'Messy Scholar Bun', 'hairstyle', 578,84,28,18,
             'A loosely tied bun with stray strands framing the face — equal parts intellect and quiet confidence.'
      WHERE NOT EXISTS (SELECT 1 FROM item WHERE id = 10);
      """,
      """
      INSERT INTO player_item (id, equipped, type, player_id, item_id)
      SELECT 1,1,'outfit',1, 5
      WHERE NOT EXISTS (SELECT 1 FROM player_item WHERE id = 1);
      INSERT INTO player_item (id, equipped, type, player_id, item_id)
      SELECT 2,1,'hairstyle',1, 8
      WHERE NOT EXISTS (SELECT 1 FROM player_item WHERE id = 2);
      """
    };

    await using var transaction = await connection.BeginTransactionAsync();
    foreach (var sql in inserts)
    {
      await using var cmd = connection.CreateCommand();
      cmd.CommandText = sql;
      cmd.Transaction = (SqliteTransaction)transaction;
      await cmd.ExecuteNonQueryAsync();
    }

    await transaction.CommitAsync();
  }
}