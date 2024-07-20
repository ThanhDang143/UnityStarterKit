using System;
using System.Collections;
using ZBase.Collections.Pooled.Generic;
using UnityEngine;

public class NotificationService
{
    private static NotificationService _instance;

    public static NotificationService Instance
    {
        get { return _instance ??= new NotificationService(); }
    }

    public delegate void UpdateDelegator();
    public delegate void Delegator(Hashtable data);
    public delegate void DelegatorBadge(int count);

    private ValueDictionary<string, UpdateDelegator> _delegateMap;
    private ValueDictionary<string, Delegator> _maps;
    // readonly Dictionary<BadgeType, DelegatorBadge> _badges;
    // readonly Dictionary<BadgeType, int> _badgeData;

    private NotificationService()
    {
        _delegateMap = ValueDictionary<string, UpdateDelegator>.Create();
        _maps = ValueDictionary<string, Delegator>.Create();
        // _badges = new Dictionary<BadgeType, DelegatorBadge>();
        // _badgeData = new Dictionary<BadgeType, int>();
    }

    public void Add(string subject, UpdateDelegator delegator)
    {
        _delegateMap.TryAdd(subject, delegate () { });
        _delegateMap[subject] += delegator;
    }

    public void Add(string subject, Delegator delegator)
    {
        _maps.TryAdd(subject, data => { });
        _maps[subject] += delegator;
    }

    public void Remove(string subject, UpdateDelegator delegator)
    {
        if (_delegateMap.ContainsKey(subject) == false) return;
        _delegateMap[subject] -= delegator;
    }

    public void Remove(string subject, Delegator delegator)
    {
        if (_maps.ContainsKey(subject) == false) return;
        _maps[subject] -= delegator;
    }

    public void Post(string subject)
    {
        if (_delegateMap.TryGetValue(subject, out var value))
        {
            if (value != null)
                foreach (var @delegate in value.GetInvocationList())
                {
                    var delegator = (UpdateDelegator)@delegate;
                    try
                    {
                        delegator();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
        }
    }

    public void Post(string subject, Hashtable data)
    {
        if (_maps.TryGetValue(subject, out var map))
        {
            if (map != null)
                foreach (var @delegate in map.GetInvocationList())
                {
                    var delegator = (Delegator)@delegate;
                    try
                    {
                        delegator.Invoke(data);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
        }
    }

    // #region Badge
    // public void AddBadge(BadgeType type, DelegatorBadge delegator)
    // {
    //     _badges.TryAdd(type, data => { });
    //     _badges[type] += delegator;
    // }

    // public void RemoveBadge(BadgeType type, DelegatorBadge delegator)
    // {
    //     if (_badges.ContainsKey(type) == false) return;
    //     _badges[type] -= delegator;
    // }

    // public int GetBadgeData(BadgeType type)
    // {
    //     int result = 0;
    //     if (_badgeData.TryGetValue(type, out var badge)) result = badge;
    //     return result;
    // }

    // public void PostBadge(BadgeType type, int count)
    // {
    //     //store data
    //     if (_badgeData.ContainsKey(type))
    //     {
    //         _badgeData[type] = count;
    //     }
    //     else
    //     {
    //         _badgeData.Add(type, count);
    //     }

    //     if (_badges.TryGetValue(type, out var map))
    //     {
    //         if (map != null)
    //             foreach (var @delegate in map.GetInvocationList())
    //             {
    //                 var delegator = (DelegatorBadge)@delegate;
    //                 try
    //                 {
    //                     delegator.Invoke(count);
    //                 }
    //                 catch (Exception e)
    //                 {
    //                     Debug.LogException(e);
    //                 }
    //             }
    //     }
    // }
    // #endregion
}
