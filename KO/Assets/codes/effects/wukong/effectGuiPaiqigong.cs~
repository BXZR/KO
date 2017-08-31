using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectGuiPaiqigong : effectBasic {

	  
	GameObject qigongbo;
	Vector3 forwardNow;
	public float lastingTime =0.25f;
	private AudioSource theSource;//声音音源引用保存
	GameObject theShield;//释放龟派气功的时候有20%伤害减免的护盾
	float theSpeed = 20f;//冲击波的移动速度
	float theDamageMinusPercent = 0.45f;

	void Start () 
	{

		Init ();//进行初始化
	}

	public override void Init ()
	{
		theEffectName = "龟派气功";
		theEffectInformation ="蓄气后发射龟派气功造成远程重度伤害\n命中可额外恢复3%最大生命值\n发射气功的"+lastingTime+"秒内拥有"+theDamageMinusPercent*100+"%伤害减免\n每1%斗气提供0.25额外伤害";

		makeStart ();
		//Debug .Log(this.thePlayer .name +"的额外效果："+ this.getInformation());
		//GuiPaiQiGong();

		theShield = (GameObject)GameObject .Instantiate(  (GameObject)Resources.Load ("effects/guipaiqigongshield"));
		theShield.transform.parent = thePlayer.transform;
		theShield.transform.localPosition = new Vector3 (0f,0.5f,-0.1f);
		//theShield .transform .localScale*= thePlayer.transform.localScale.y;
		theShield.GetComponentInChildren<ParticleSystem> ().startSize *= thePlayer.transform.localScale.y;

		 GuiPaiQiGong();	 
	} 
 
	public  void effectOnUpdatePrivate ()
	{
		if (qigongbo) {
			qigongbo.transform.Translate (forwardNow * theSpeed * Time.deltaTime);
 

			lastingTime -= Time.deltaTime;
			if ((lastingTime < 0 || thePlayer.isAlive == false) && (theSource == null || theSource.isPlaying == false)) {
				Destroy (qigongbo.gameObject);
				if (theShield)
					Destroy (theShield .gameObject);
				Destroy (this.gameObject.GetComponent (this.GetType ()));//为了保证灵活性，也还是应该使用人工计时的方法

			}
		}
	}
 
	public override void OnAttack (PlayerBasic aim)
	{
		thePlayer = this.GetComponentInChildren<PlayerBasic> ();
		thePlayer.ActerHp += thePlayer.ActerHpMax * 0.03f;
		aim.ActerHp -= 25f * (thePlayer.ActerSp / thePlayer.ActerSpMax);
	}

	public override void OnBeAttack (float damage = 0)
	{
		thePlayer = this.GetComponentInChildren<PlayerBasic> ();
		this.thePlayer.ActerHp += damage * theDamageMinusPercent;
	}

	public override void effectDestory ()
	{
		 
		Destroy (qigongbo);
		Destroy (theShield .gameObject);
	}

	void OnDestroy()
	{
		effectDestory ();
	}
 
	void Update ()
	{
		effectOnUpdatePrivate ();
 
	}

 
	void GuiPaiQiGong()
	{
 
		qigongbo= GameObject.Instantiate ((GameObject)Resources.Load ("effects/wukongguipaiqigong"));
		qigongbo .GetComponentInChildren<ParticleSystem> ().startSize *= thePlayer.transform.localScale.y;
		qigongbo.GetComponent <extraWeapon> ().setPlayer (this.thePlayer);
		qigongbo.transform.parent = this.thePlayer.transform;
		qigongbo .transform .localScale*= thePlayer.transform.localScale.y;
		qigongbo.transform.position = new Vector3 (thePlayer .transform .position .x,thePlayer .transform .position.y +0.7f* thePlayer .transform .localScale .y ,thePlayer .transform .position .z-0.1f);
		forwardNow = new Vector3(0,0 ,this.thePlayer .transform .forward .z);
	}
}
