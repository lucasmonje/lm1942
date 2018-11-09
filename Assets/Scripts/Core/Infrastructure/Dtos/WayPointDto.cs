using System;
using Core.Domain.Entities;
using Core.Domain.Models;
using Core.Domain.Utils;
using UnityEngine;

namespace Core.Infrastructure.Dtos
{
    [Serializable]
    public class WayPointDto
    {
        public Position Position;
        public AirplaneAction Action;

        public WayPointDto(Position position, AirplaneAction action)
        {
            Position = position;
            Action = action;
        }

        public WayPointDto()
        {
        }
    }
}