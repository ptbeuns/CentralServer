namespace CentralServer
{
    public class Coupe
    {
        public int CoupeId { get; private set; }

        public int CoupeOccupation { get; private set; }

        public Coupe(int coupeId)
        {
            CoupeId = coupeId;
        }

        public void UpdateOccupation(int occupation)
        {
            if (occupation < 0)
            {
                //TODO: decide what to do here
            }

            CoupeOccupation = occupation;
        }

    }
}