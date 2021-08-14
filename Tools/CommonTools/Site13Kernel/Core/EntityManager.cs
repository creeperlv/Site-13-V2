using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Core
{
    public class EntityManager
    {
        List<Memory<IComponent>> entities;
        Dictionary<Entity,Object> Entity_Gameobject;
        public void Init()
        {
            Entity_Gameobject = new Dictionary<Entity, object>(100);
            entities = new List<Memory<IComponent>>(100);
        }
        public List<Entity> GetEntities<T>()
        {
            List<Entity> _collection=new List<Entity>();
            foreach (var item in entities)
            {
                if (item.Span[0] is Entity e)
                {
                    foreach (var component in item.Span)
                    {
                        if (component is T)
                        {
                            _collection.Add(e);
                            break;
                        }
                    }

                }
            }
            return _collection;
        }
        public List<Entity> GetEntities<T, V>()
        {
            List<Entity> _collection=new List<Entity>();
            foreach (var item in entities)
            {
                if (item.Span[0] is Entity e)
                {
                    bool hit0=false;
                    bool hit1=false;

                    foreach (var component in item.Span)
                    {
                        if (component is T)
                        {
                            hit0 = true;
                        }
                        if (component is V)
                        {
                            hit1 = true;
                        }
                        if (hit0 && hit1)
                        {
                            _collection.Add(e);
                            break;
                        }
                    }

                }
            }
            return _collection;
        }
        public List<Entity> GetEntities<T, V, U>()
        {
            List<Entity> _collection=new List<Entity>();
            foreach (var item in entities)
            {
                if (item.Span[0] is Entity e)
                {
                    bool hit0=false;
                    bool hit1=false;
                    bool hit2=false;

                    foreach (var component in item.Span)
                    {
                        if (component is T)
                        {
                            hit0 = true;
                        }
                        if (component is V)
                        {
                            hit1 = true;
                        }
                        if (component is U)
                        {
                            hit2 = true;
                        }
                        if (hit0 && hit1 && hit2)
                        {
                            _collection.Add(e);
                            break;
                        }
                    }

                }
            }
            return _collection;
        }

        public void PushEntity(Memory<IComponent> memory)
        {
            var e=(Entity)memory.Span[0];
            e.ID = RandomTool.NextInt();
            memory.Span[0] = e;
            entities.Add(memory);
        }
        public void Destory(Entity entity)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (((Entity) entities[i].Span[0]).ID == entity.ID)
                {
                    entities.RemoveAt(i);
                    Entity_Gameobject.Remove(entity);
                    break;
                }
            }
        }
        public void DestoryAll()
        {
            Entity_Gameobject.Clear();
            entities.Clear();
        }
        public Entity CreateInstanceCopy(Entity entity, Object BindedObj)
        {
            Entity __ENTITY=new Entity();
            __ENTITY.ID = RandomTool.NextInt();
            for (int i = 0; i < entities.Count; i++)
            {
                if (((Entity) entities[i].Span[0]).ID == entity.ID)
                {
                    var OldC=entities[i];
                    Memory<IComponent> NewC=new Memory<IComponent>(OldC.Span.ToArray());
                    NewC.Span[0] = __ENTITY;
                    entities.Add(NewC);
                    break;
                }
            }
            Entity_Gameobject.Add(__ENTITY, BindedObj);
            return __ENTITY;
        }
        public void SetComponent<T>(Entity entity, T component) where T : struct, IComponent
        {
            foreach (var item in entities)
            {
                if (item.Span[0] is Entity e)
                {
                    if (e.ID == entity.ID)
                    {
                        var sp= item.Span;
                        for (int i = 0; i < item.Length; i++)
                        {
                            if (sp[i] is T)
                            {
                                sp[i] = component;
                            }
                        }
                        break;
                    }
                }
            }
        }
        public void AddComponent<T>(Entity entity, T component) where T : struct, IComponent
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (((Entity) entities[i].Span[0]).ID == entity.ID)
                {
                    var OldC=entities[i];
                    IComponent[] _arr= new IComponent[OldC.Length+1];
                    Memory<IComponent> NewC=new Memory<IComponent>(_arr);
                    OldC.CopyTo(NewC);
                    NewC.Span[NewC.Length - 1] = component;
                    entities[i] = NewC;
                    break;
                }
            }

        }
        public T? GetComponent<T>(Entity entity) where T : struct, IComponent
        {
            foreach (var item in entities)
            {
                if (item.Span[0] is Entity e)
                {
                    if (e.ID == entity.ID)
                    {
                        foreach (var component in item.Span)
                        {
                            if (component is T)
                            {
                                return (T?) component;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
    public struct Entity : IComponent
    {
        public int ID;
        public int Version;
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
