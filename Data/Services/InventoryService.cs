﻿using inventarioApi.Data.DTO;
using inventarioApi.Data.Helpers;
using inventarioApi.Data.Models;
using InventarioApi;
using Microsoft.EntityFrameworkCore;

namespace inventarioApi.Data.Services
{
    public class InventoryService
    {
        private readonly InventarioContext _context;

        //GET
        public InventoryService(InventarioContext context)
        {
            _context = context;
        }
        public async Task<List<Inventory>> GetInventoriesAsync()
        {
            return await _context.Inventories.ToListAsync();
        }
        public async Task<Inventory> GetInventoryById(int ID_INVENTORY)
        {
            //return await _context.Inventory.FromSqlRaw("SELECT * FROM \"INVENTORY\" WHERE \"ID_INVENTORY\" = {0}", INVENTORY).FirstOrDefaultAsync();
            var result = await _context.Inventories.FromSqlRaw("SELECT * FROM \"INVENTORY\" WHERE \"ID_INVENTORY\" = {0}", ID_INVENTORY).FirstOrDefaultAsync();

            if (result != null)
            {
                return result;
            }
            else
            {
                throw new Exception("Null value");
            }
        }
        public async Task<List<UserInventory>> GetUserInventories(int ID_USER)
        {
            return await _context.UserInventories.FromSqlRaw("SELECT * FROM \"USER_INVENTORY\" WHERE \"USER\" = {0}", ID_USER).ToListAsync();
        }

        //POST
        public async Task<UserInventory> CreateInventory(Inventory INVENTORY, int ID_USER)
        {

            var inventoryEntity = new Inventory
            {
                //ID_INVENTORY,
                //DATE,
                TITLE = INVENTORY.TITLE,
                DESCRIPTION = INVENTORY.DESCRIPTION,
                IMAGE = INVENTORY.IMAGE,
                DATE = DateTimeOffset.UtcNow
            };

            await _context.Inventories.AddAsync(inventoryEntity);

            try
            {
                await _context.SaveChangesAsync();
                int newInventoryId = inventoryEntity.ID_INVENTORY;

                var userInventoryEntity = new UserInventory
                {
                    USER = ID_USER,
                    INVENTORY = newInventoryId,
                    TYPE = UserInventoryType.OWNER,
                    DATE = DateTimeOffset.UtcNow
                };

                var createUserInventory = await _context.UserInventories.AddAsync(userInventoryEntity);
                await _context.SaveChangesAsync();
                return userInventoryEntity;
            }catch (Exception ex)
            {
                throw ex;
            }
        }

        //PUT

        //DELETE

    }
}
