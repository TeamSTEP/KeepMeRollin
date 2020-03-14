//enums used for AI behaviors
public enum EnemyStates
{
    Passive, // default state, will follow normal route
    Detect, // AI sees something unusual, will move to point of interest
    Aggressive // AI sees player, will run to catch the player
}