

using OTS2026_PT1_GrupaA;
using System;
using System.IO;
using static OTS2026_PT1_GrupaA.Game;

namespace OTS2026_PT1_GrupaA.Exceptions
{
    public class InvalidPlayerPositionException: Exception
    {
        public InvalidPlayerPositionException()
        {

        }

        public InvalidPlayerPositionException(string message) : base(message)
        {

        }
    }
}
