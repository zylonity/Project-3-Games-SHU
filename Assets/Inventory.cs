using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//
//private class Item
//{
//    public string name = "NONE";
//    public int id = 0;
//    public int count = 0;
//    public float durability = 1.0f;
//    public void Update()
//    {
//
//    }
//}
public class Inventory : MonoBehaviour
{
    public class Items
    {
        public enum Item { None, Poncho, Bandage, Torch }
        private Item _itemID = Item.None;
        private int stack = 1;
        public string name = "NONE";
        public int Durability = 100;
        public int durabilityLeft = 0;
        public int Number = 1;
        public float DurPercent = 0;
        private int breakPercent = 0;
        private bool timeBrakable = false;
        private float breakTimer = 0.0f;
        private float breakTime = 1.0f;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item">only three are available</param>
        /// <param name="percent"> from 1 to 100</param>
        /// <returns> has item been broken for bandage is useful for heal system</returns>
        public bool Update(bool inUse = false)
        {
            bool used = false;
            if (inUse)
            {
                if (timeBrakable && Number >= 1)
                {
                    breakTimer += Time.deltaTime;
                    if (breakTimer > breakTime)
                    {
                        breakTimer = 0.0f;
                        used = UseItem(breakPercent);
                    }
                };
                DurPercent = (float)durabilityLeft / Durability;
            }
            return used;
        }
        public void SetupItem(Item item, int _Number = 1, int _Durability = 100, int _stack = 1, bool _timeBreakable = false, float _breakTime = 0.0f, int _breakPercent = 0)
        {
            timeBrakable = _timeBreakable;
            stack = _stack;
            Number = _Number;
            Durability = _Durability;
            durabilityLeft = Durability;
            if (_timeBreakable)
            {
                breakTime = _breakTime;
                breakPercent = _breakPercent;
            }
            _itemID = item;
            name = item.ToString();
            DurPercent = (float)durabilityLeft / Durability;
        }
        public bool UseItem(int percent = 10)
        {
            bool used = false;
            if (Number >= 1)
            {
                durabilityLeft -= percent;
                if (durabilityLeft <= 0)
                {
                    used = true;
                    durabilityLeft = Durability;
                    --Number;
                }
            }
            DurPercent = (float)durabilityLeft / Durability;
            return used;
        }
        public void NotFinishedUsageCheck()
        {
            if(timeBrakable)
                if(durabilityLeft > 0 && durabilityLeft < Durability)
                    durabilityLeft = Durability;
            DurPercent = (float)durabilityLeft / Durability;
        }
        public bool AddItem(short number = 1)
        {
            bool added = false;
            if (Number + number <= stack)
            {
                Number += number;
                added = true;
            }
            return added;
        }
    }
    [SerializeField, Range(0, 64)] private int ponchoNumber = 1;
    [SerializeField, Range(0, 64)] private int torchNumber = 1;
    [SerializeField, Range(0, 64)] private int bandageNumber = 1;
    public bool AcidRain = false;
    [SerializeField, Range(0, 10)] private float messageTime = 1.0f;
    private float messageTimer = 0.0f;
    private bool startMessageTimer = false;
    private GameObject _playerUI = null;
      
    [SerializeField] private PlayerController _playerController = null;

    [SerializeField] private GameObject bandage_count_text_obj = null;
    [SerializeField] private GameObject poncho_count_text_obj = null;
    [SerializeField] private GameObject torch_count_text_obj = null;

    [SerializeField] private GameObject bandage_img_obj = null;
    [SerializeField] private GameObject poncho_img_obj = null;
    [SerializeField] private GameObject torch_img_obj = null;

    [SerializeField] private GameObject bandage_DurBar = null;
    [SerializeField] private GameObject poncho_DurBar = null;
    [SerializeField] private GameObject torch_DurBar = null;

    [SerializeField] private TextMeshProUGUI _messege = null;

    private Image bandage_img = null;
    private Image poncho_img = null;
    private Image torch_img = null;

    TextMeshProUGUI bandage_count_text = null;
    TextMeshProUGUI poncho_count_text  = null;
    TextMeshProUGUI torch_count_text = null;

    public Items Torch = new Items();
    public Items Bandage = new Items();
    public Items Poncho = new Items();
    // Start is called before the first frame update
    void Start()
    {
        bandage_img = bandage_img_obj.GetComponent<Image>();
        poncho_img = poncho_img_obj.GetComponent<Image>();
        torch_img = torch_img_obj.GetComponent<Image>();

        bandage_count_text = bandage_count_text_obj.GetComponent<TextMeshProUGUI>();
        poncho_count_text = poncho_count_text_obj.GetComponent<TextMeshProUGUI>();
        torch_count_text = torch_count_text_obj.GetComponent<TextMeshProUGUI>();

        _playerUI = GetComponent<GameObject>();

        Poncho.SetupItem(Items.Item.Poncho, ponchoNumber, 100, 10, true, 5.0f, 10);
        Torch.SetupItem(Items.Item.Torch, torchNumber, 100, 10, true, 10, 5);
        Bandage.SetupItem(Items.Item.Bandage, bandageNumber, 100, 10, true, 0.05f,2);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ItemController itm = null;
        if (collision.CompareTag("Bandage"))
        {
            itm = collision.GetComponent<ItemController>();
            if (!itm.destroy && PickItem(Inventory.Items.Item.Bandage))
                itm.destroy = true;
        }
        else if (collision.CompareTag("Torch"))
        {
            itm = collision.GetComponent<ItemController>();
            if (!itm.destroy && PickItem(Inventory.Items.Item.Torch))
                itm.destroy = true;
        }
        else if (collision.CompareTag("Poncho"))
        {
            itm = collision.GetComponent<ItemController>();
            if (!itm.destroy && PickItem(Inventory.Items.Item.Poncho))
                itm.destroy = true;
        }

    }
    // Update is called once per frame
    void Update()
    {
        Poncho.Update(_playerController.ponchoOn && AcidRain);

        if (_playerController.healing && _playerController.playerHealth != _playerController.maxHealth)
        {
            if (Bandage.Update(true))
            {
                _playerController.playerHealth += _playerController.bandageHeals;
                if (_playerController.playerHealth > _playerController.maxHealth)
                    _playerController.playerHealth = _playerController.maxHealth;
            }
        }
        else
            Bandage.NotFinishedUsageCheck();

        Torch.Update(_playerController.holdTorch);

        poncho_count_text.text = Poncho.Number.ToString();
        bandage_count_text.text = Bandage.Number.ToString();
        torch_count_text.text = Torch.Number.ToString();

        poncho_DurBar.transform.localScale = new Vector3(1.0f * Poncho.DurPercent, 1.0f, 1.0f);

        bandage_DurBar.transform.localScale = new Vector3(1.0f * Bandage.DurPercent, 1.0f, 1.0f);

        torch_DurBar.transform.localScale = new Vector3(1.0f * Torch.DurPercent, 1.0f, 1.0f);

        if (startMessageTimer)
        {
            startMessageTimer = false;
            if (messageTimer > messageTime)
            {
                messageTimer = 0.0f;
                _messege.text = "";
            }
        }
    }
    public bool PickItem(Items.Item item) 
    {
        bool ret = false;
        if (_playerController.picking)
        {
            switch (item)
            {
                case Items.Item.None:
                    _messege.text = "";
                    break;
                case Items.Item.Poncho:
                    ret = Poncho.AddItem();
                    if (ret)
                    {
                        _messege.text = "\"" + Poncho.name + "\" Picked!";
                    }
                    break;
                case Items.Item.Bandage:
                    ret = Bandage.AddItem();
                    if (ret)
                    {
                        _messege.text = "\"" + Bandage.name + "\" Picked!";
                    }
                    break;
                case Items.Item.Torch:
                    ret = Torch.AddItem();
                    if (ret)
                    {
                        _messege.text = "\"" + Torch.name + "\" Picked!";
                    }
                    break;
            }
            if (item != Items.Item.None && !ret)
                _messege.text = "Inventory is full!";
            else
                startMessageTimer = true;
        }
        else
        {
            switch (item)
            {
                case Items.Item.None:
                    _messege.text = "";
                    break;
                case Items.Item.Poncho:
                    _messege.text = "Press \'E\' to pick a \"" + Poncho.name + "\"";
                    break;
                case Items.Item.Bandage:
                    _messege.text = "Press \'E\' to pick \"" + Bandage.name + "\"";
                    break;
                case Items.Item.Torch:
                    _messege.text = "Press \'E\' to pick a \"" + Torch.name + "\"";
                    break;
            }
            if(item != Items.Item.None)
            {
                startMessageTimer = false;
                messageTimer = 0.0f;
            }
        }
        return ret;
    }
}
