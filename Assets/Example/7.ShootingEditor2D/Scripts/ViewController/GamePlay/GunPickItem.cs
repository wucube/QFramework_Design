using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace ShootingEditor2D
{
    public class GunPickItem : ShootingEditor2DController
    {
        public new string name;
        public int bulletCountInGun;
        public int bulletCountOutGun;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
                this.SendCommand(new PickGunCommand(name,bulletCountInGun,bulletCountOutGun));
            
            Destroy(gameObject);
        }
    }

}
