using UnityEngine;

public struct Positions
{
    public Vector3 position;
    public bool availability;
    public Positions(Vector3 m_position, bool m_availability)
    {
        position = m_position;
        availability = m_availability;
    }
}
