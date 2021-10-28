using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class Loto : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI userMoneyPro;
    [SerializeField] int userMoney = 1000;
    
    [SerializeField] TextMeshProUGUI lastPrisePro;
    

    [SerializeField] TextMeshProUGUI totalPrisesPro;
    int totalPrises = 0;
  
    [SerializeField] GameObject[] computerNumsPro;
    int[] computerNums;
    
    [SerializeField] GameObject[] userNumsPro;
    int[] userNums;
    

    Dictionary<float, int> prises1 = new Dictionary<float, int>();
    Dictionary<float, int> prises2 = new Dictionary<float, int>();
    Dictionary<float, int> prises3 = new Dictionary<float, int>();
    readonly System.Random r = new System.Random();

    int rows = 0;
    [SerializeField] Sprite[] images;
    [SerializeField] GameObject[] vfx;
    [SerializeField] AudioClip appearSFX;
    [SerializeField] AudioClip prizeSFX;
    [SerializeField] AudioClip[] matchesSFX;
    Color[] colors;
    int[] bestColors;
    bool easy = false;
    bool medium = false;
    bool hardd = false;

    void Start()
    {
        colors = new Color[5];
        setColors();
        hideNums(computerNumsPro);
        hideNums(userNumsPro);
        lastPrisePro.gameObject.SetActive(false);
        setUpPrises();
        bestColors = new int[5];
    }

   
    void Update()
    {
        
    }

    private void setColors() {
        colors[0] = new Color(255,0,0,255);
        colors[1] = new Color(0, 255, 0, 255);
        colors[2] = new Color(255, 255, 0, 255);
        colors[3] = new Color(0, 0, 255, 255);
        colors[4] = new Color(255, 255, 255, 255);
    }
    public void setRowsInput(string input) {
        if (input.Length == 0 || input == "" || input == null)
            rows = 0;
        else
        {
            int num;
            bool ok = int.TryParse(input, out num);
            if (ok)
                rows = num;
        }  
    }
    

    public int[] lotoRandom(int n)
    {
        int[] arr = new int[n];
        do
        {
            for (int i = 0; i < n; i++)
                arr[i] = r.Next(0, 10);
        } while (!(differentNumbers(arr)));
        return arr;
    }

    public int[] colorRandom(int n)
    {
        int[] arr = new int[n];
            for (int i = 0; i < n; i++)
                arr[i] = r.Next(0, 4);
        return arr;
    }

    private void setUpPrises() {
        prises1.Add(0f, 0);
        prises1.Add(1f, 1);
        prises1.Add(2f, 2);
        prises1.Add(3f, 3);
        prises1.Add(4f, 10);
        prises1.Add(5f, 250);

        prises2.Add(0f, 0);
        prises2.Add(1f, 3);
        prises2.Add(2f, 13);
        prises2.Add(3f, 100);
        prises2.Add(4f, 1200);
        prises2.Add(5f, 30240);

        prises3.Add(0f, 0);
        prises3.Add(1f, 9);
        prises3.Add(2f, 158);
        prises3.Add(3f, 5500);
        prises3.Add(4f, 250000);
        prises3.Add(5f, 31000000);
    }

    public bool differentNumbers(int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            for (int j = i + 1; j < arr.Length; j++)
            {
                if (arr[i] == arr[j])
                    return false;
            }
        }
        return true;
    }

    
    public void randomComputer() {
        
        computerNums = lotoRandom(5);
        hideNums(computerNumsPro);
        StartCoroutine(appearNums(computerNumsPro, computerNums));
    }
   

    public IEnumerator appearNums(GameObject[] nums,int[] idx)
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        for (int i = 0; i < nums.Length; i++)
        { 
            nums[i].GetComponent<SpriteRenderer>().sprite = images[idx[i]];
            nums[i].GetComponent<Appear>().isAppear = true;
            nums[i].gameObject.SetActive(true);
            Invoke("appearSound", 0.1f);
            yield return wait;
        }
    }
    public void appearSound() {
        AudioSource.PlayClipAtPoint(appearSFX, Camera.main.transform.position);
    }
    public void hideNums(GameObject[] nums) {
        for (int i = 0; i < nums.Length; i++)
        {
           nums[i].gameObject.SetActive(false);
           nums[i].GetComponent<Appear>().isAppear = false;
        }
        
    }

    public void changeToEasy() {
        easy = true;
        medium = false;
        hardd = false;
    }
    public void changeToMedium()
    {
        easy = false;
        medium = true;
        hardd = false;
    }
    public void changeToHard()
    {
        easy = false;
        medium = false;
        hardd = true;
    }
    public void resetLevel()
    {
        easy = false;
        medium = false;
        hardd = false;   
    }

    public void resetColors() {
        for (int i = 0; i < colors.Length; i++)
        {
            computerNumsPro[i].GetComponent<SpriteRenderer>().color = colors[4];
            userNumsPro[i].GetComponent<SpriteRenderer>().color = colors[4];
        }
    }

    public void randomUser12()
    {
        if (userMoney >= rows && rows > 0)
        {
            Destroy(GameObject.FindWithTag("VFX"));
            lastPrisePro.gameObject.SetActive(false);
            totalPrisesPro.gameObject.SetActive(false);
            int lastPrise = 0;
            userMoney -= rows;
            hideNums(userNumsPro);
            resetColors();
            randomComputer();
            float max = -1;
            int[] userNums1 = lotoRandom(5);
            max = matches12(userNums1);
            
            if (easy)
            {
                
                lastPrise += prises1[max];
                totalPrises += prises1[max];
            }
            else if (medium)
            {
                
                lastPrise += prises2[max];
                totalPrises += prises2[max];
            }

            int[] userNums2 = new int[5];
           
            for (int i = 0; i < rows-1; i++)
            {
                float count = -1;
                userNums2 = lotoRandom(5);
                count = matches12(userNums2);
                
                if (easy)
                {
                    
                    lastPrise += prises1[count];
                    totalPrises += prises1[count];
                }
                else if (medium)
                {
                    
                    lastPrise += prises2[count];
                    totalPrises += prises2[count];
                }

                if (count >= max)
                {
                    
                    max = count;
                    userNums1 = userNums2;
                    
                }
            }
            userNums = userNums1;
  
            StartCoroutine(appearNums(userNumsPro, userNums));
        if(easy)
            Invoke("remarkSame1", 4f);
        else if(medium)
            Invoke("remarkSame2", 4f);
        

        resetLevel();

           totalPrisesPro.text = totalPrises.ToString();
           userMoneyPro.text = userMoney.ToString();
           lastPrisePro.text = lastPrise.ToString();
          }
         else Debug.Log("User dont have enough money to this bet");
    }

    public void randomUser3()
    {
        if (userMoney >= rows && rows > 0)
        {
            Destroy(GameObject.FindWithTag("VFX"));
            lastPrisePro.gameObject.SetActive(false);
            totalPrisesPro.gameObject.SetActive(false);
            int lastPrise = 0;
            userMoney -= rows;
            hideNums(userNumsPro);
            resetColors();
            randomComputer();
            int[] computerColors = colorRandom(5);
            float max = 0;

            int[] userNums1 = lotoRandom(5);
            int[] userNums1Pro = colorRandom(5);
            max = matches3(userNums1, userNums1Pro, computerColors);
            lastPrise += prises3[max];
            totalPrises += prises3[max];
            int[] userNums2 = new int[5];
            int[] userNums2Pro = new int[5]; 
            for (int i = 0; i < rows-1; i++)
            {
                float count = 0;
                userNums2 = lotoRandom(5);
                userNums2Pro = colorRandom(5); 
                count = matches3(userNums2, userNums2Pro, computerColors);
                
                lastPrise += prises3[count];
                totalPrises += prises3[count];

                if (count >= max)
                { 
                    max = count;
                    userNums1 = userNums2;
                    userNums1Pro = userNums2Pro;
                }
            }

            userNums = userNums1;
            putColors(userNums1Pro, computerColors);
            StartCoroutine(appearNums(userNumsPro, userNums));
            Invoke("remarkSame3", 4f);

            resetLevel();
            totalPrisesPro.text = totalPrises.ToString();
            userMoneyPro.text = userMoney.ToString();
            lastPrisePro.text = lastPrise.ToString();
        }
        else Debug.Log("User dont have enough money to this bet");
    }


    public void putColors(int[] userNum1Pro, int[] computerColors)
    {
        for (int i = 0; i < userNumsPro.Length; i++)
        {
            userNumsPro[i].GetComponent<SpriteRenderer>().color = colors[userNum1Pro[i]];
            computerNumsPro[i].GetComponent<SpriteRenderer>().color = colors[computerColors[i]];
        }
    }

    public void remarkSame1() {
        StartCoroutine(showVFX1());   
    }
    public IEnumerator showVFX1()
    {
        int count = 0;
        WaitForSeconds wait = new WaitForSeconds(0.7f);
        for (int i = 0; i < userNums.Length; i++)
        {
            for (int j = 0; j < computerNums.Length; j++)
            {
                if (userNums[i] == computerNums[j])
                {
                    Instantiate(vfx[count], userNumsPro[i].transform.position, Quaternion.identity);
                    Instantiate(vfx[count], computerNumsPro[j].transform.position, Quaternion.identity);
                    AudioSource.PlayClipAtPoint(matchesSFX[count], Camera.main.transform.position);
                    count++;
                    yield return wait;
                }
            }
        }
        Invoke("appearMoney", 0.7f);
    }

    public void appearMoney() {
        lastPrisePro.gameObject.SetActive(true);
        totalPrisesPro.gameObject.SetActive(true);
        AudioSource.PlayClipAtPoint(prizeSFX, Camera.main.transform.position);
    }

    public void remarkSame2()
    {
        StartCoroutine(showVFX2());
    }
    public IEnumerator showVFX2()
    {
        int count = 0;
        WaitForSeconds wait = new WaitForSeconds(0.7f);
        for (int i = 0; i < userNums.Length; i++)
        {
            
                if (userNums[i] == computerNums[i])
                {
                    Instantiate(vfx[count], userNumsPro[i].transform.position, Quaternion.identity);
                    Instantiate(vfx[count], computerNumsPro[i].transform.position, Quaternion.identity);
                    AudioSource.PlayClipAtPoint(matchesSFX[count], Camera.main.transform.position);
                    count++;
                    yield return wait;
                }
            
        }
        Invoke("appearMoney", 0.7f);
    }

    public void remarkSame3()
    {
        StartCoroutine(showVFX3());
    }
    public IEnumerator showVFX3()
    {
      //  int count = 0;
        WaitForSeconds wait = new WaitForSeconds(0.7f);
        for (int i = 0; i < userNums.Length; i++)
        {

            if (userNums[i] == computerNums[i] &&
                userNumsPro[i].GetComponent<SpriteRenderer>().color == computerNumsPro[i].GetComponent<SpriteRenderer>().color)
            {
                Instantiate(vfx[5], userNumsPro[i].transform.position, Quaternion.identity);
                Instantiate(vfx[5], computerNumsPro[i].transform.position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(matchesSFX[5], Camera.main.transform.position);
               // count++;
                yield return wait;
            }

        }
        Invoke("appearMoney", 0.7f);
    }

    public float matches12(int[] arr)
    {
        if (easy)
            return matches1(arr);
        else if (medium)
            return matches2(arr);
        return matches1(arr);
    }

    public float matches1(int[] arr)
    {
        float count = 0;
        for (int i = 0; i < computerNums.Length; i++)
        {
            for (int j = 0; j < arr.Length; j++)
            {
                if (computerNums[i] == arr[j])
                    count++;
            }
        }
        
        return count;
    }

    public float matches2(int[] arr)
    {
        float count = 0;
        for (int i = 0; i < computerNums.Length; i++)
        {       
                if (computerNums[i] == arr[i])
                    count++;
        }
        return count;
    }

    public float matches3(int[] arr, int[] arrPro, int[] computerColors)
    {
        float count = 0;
        for (int i = 0; i < computerNums.Length; i++)
        { 
            if ((computerNums[i] == arr[i]) && (arrPro[i] == computerColors[i]))
                count++;
        }
        return count;
    }

}
