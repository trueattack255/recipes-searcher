using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using Api.Interfaces;
using Api.Models;

namespace Api.Services
{
    public class ReceiptLogService : ILogService
    {
        private readonly NoteContext _context;

        public ReceiptLogService(IOptions<Settings> settings)
        {
            _context = new NoteContext(settings);
        }

        public Note AddNote(string value, string status)
        {
            try
            {
                var filter = Builders<Note>.Filter.Eq(it => it.Body, value);
                var result = _context.Receipts.UpdateOne(filter, Builders<Note>.Update
                    .Set("date", DateTime.Now.ToString()));

                if (result.MatchedCount == 0)
                    _context.Receipts.InsertOne(new Note
                    {
                        Body = value,
                        Code = status
                    });

                return _context.Receipts
                    .Find(filter)
                    .FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Note GetNote(string id)
        {
            try
            {
                var filter = Builders<Note>.Filter.Eq("_id", ObjectId.Parse(id));
                return _context.Receipts
                    .Find(filter)
                    .FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
