using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using Api.Interfaces;
using Api.Models;

namespace Api.Shared
{
    public class ErrorLogService : ILogService
    {
        private readonly NoteContext _context;

        public ErrorLogService(IOptions<Settings> settings)
        {
            _context = new NoteContext(settings);
        }

        public Note AddNote(string value, string status)
        {
            try
            {
                _context.Errors.InsertOne(new Note
                {
                    Body = value,
                    Code = status
                });
            }
            catch
            {
                // ignored
            }

            return null;
        }

        public Note GetNote(string id)
        {
            try
            {
                var filter = Builders<Note>.Filter.Eq("_id", ObjectId.Parse(id));
                return _context.Errors
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
