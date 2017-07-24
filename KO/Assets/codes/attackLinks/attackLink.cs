using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum crossFadeMode { play , crossfade , crossfadeFix }
public class attackLink : MonoBehaviour {

	//所有atackLink的父类，子类的atackLink将会继承所有的方法并通过重新attackLinkEffect方法获得新的技能效果

	public string skillName;//技能名称
	public  string attackLinkString;//用字符串表示输入的键位串
	public string animationName ;//转变的动画状态机的状态名称
	protected Animator theAnimatorOfPlayer;//使用动作的人物的动画控制器
	protected CharacterController theController;//角色控制器，用于获取一些当前的状态例如是否在地面上面，此处会有很大的扩展
	protected char [] attackAray;//将输入字符串分解得到，用于逐位检测
	private bool isChangeAble = true;//是否可以更改按键，这个只有单个字符的动作可以更改，并且更改之后所有的连击字符串也会相应更改
	//**************上面是有关连招动画控制与检测的重要参数，下面是用于战斗计算等等的参数***********************/
	private PlayerBasic thePlayer;//这个招式所使用的人
	public float extraDamage = 0;//这个招式的额外伤害，所有的招式都拥有游戏人物基本的攻击力加成，然后技能本身有一个额外的伤害加成
	public float spUse =0;//使用这个招式所需要的能量值
	//public float area = 1f;//这个招式的作用范围
	//public bool isAOE =false;//这是不是一个范围伤害（暂定范围伤害就是攻击身边所有的单位，相交球）

	//因为已经确认了是二人格斗游戏，这个地方其实已经写死了
	//这段脚本会被weapon脚本在攻击得手的时候触发并且添加
	public string conNameToEMY ="";//这个招式可以为目标添加的脚本
	public string conNameToSELF ="";//这个招式可以为自身添加的脚本

	public AudioClip audioWhenAct;//在做出动作的时候就发的音效
	public AudioClip audioWhenAttack;//招式命中的时候的音效，修改的是player身上的音效
	//动作切换的方式
	public crossFadeMode crossMode = crossFadeMode.play;
	//[HideInInspector]//此效果没有必要在面板中被设置
	public int AIExtraValue = 0;//用于AI计算的额外参数
	/****************************************特殊攻击方法组****************************************************/
	//攻击检测原理：
	//用相交求获取身边所有的单位
	//用坐标的方法检测这个单位是否能够被攻击
	//对所有可以被攻击的单位进行攻击（调用player的attack方法）
//	public void attackEffect()
//	{
//		emysList.Clear ();
//		emysListToDelete.Clear ();
//		emys = Physics.OverlapSphere (this.transform .root .position, area);
//		emysList = new List<Collider> (emys);
//		int i = 0;
//		for (i = 0; i < emysList.Count; i++) 
//		{
//            if (emysList [i].GetComponent<PlayerBasic> () == null || emysList [i].gameObject == this.transform.root.gameObject) 
//			{
//			emysListToDelete.Add (emysList [i]);
//			}
//		     //检查条件：向量检查朝向问题如果不是AOE攻击，就只能够攻击到一定角度范围的单位，因为是横版游戏，貌似没有来自左右的角度，所以用的是180度
//			 if (!isAOE ) 
//			 {
//				 if(checkCanAttack (emysList [i].transform) == false) 
//			     emysListToDelete.Add (emysList [i]);
//		     }
//		}
//		for (i = 0; i < emysListToDelete.Count; i++) 
//		{
//			emysList.Remove (emysListToDelete [i]);
//		}
//
//		for (i = 0; i < emysList.Count; i++)
//		{
//			 
//			thePlayer.OnAttack (emysList [i].GetComponent <PlayerBasic>());
//		}
//
//	}
//
//	bool checkCanAttack(Transform aimTransform)//如果不是AOE那么只有在正面的对手才可以被攻击到
//	{
//		Vector3 ToAim = (aimTransform.position - this.transform.position).normalized;//序列化就不用除以模了
//		Vector3 Forward = this.transform .forward.normalized;//序列化就不用除以模了
//		float cos = ToAim .x *Forward .x + ToAim .y * Forward .y + ToAim .z *Forward.z;
//		if (cos >= 0)
//			return true;
//		return false;
//	}
	/********************************************************************************************/
	public bool IsChangeAble()
	{
		return isChangeAble;
	}

	protected void makeStart()
	{
		makeAttackArray ();
		this.theAnimatorOfPlayer = this.GetComponentInParent<Animator> ();
		this.theController = this.GetComponentInParent<CharacterController> ();
		thePlayer = this.GetComponentInParent<PlayerBasic> ();

	}

	public virtual  void makeAttackArray()//只在初始化的时候或者需要重新构建联机字符串的时候使用
	{
		if(!string .IsNullOrEmpty( attackLinkString) )//如果字符串为空则不做初始化
		attackAray = attackLinkString.ToCharArray ();//初始化数组
		if (attackAray.Length > 1)
			isChangeAble = false;
		
	}

	public  virtual void attackLinkEffect()//连招的效果在这里写
	{
		//这里其实暂时规定使用某一个攻击动作的同时不会使用另一个攻击动作
		//也就是说攻击动作之间不会发生生任何干扰
		//此外频繁第取消后摇实际上会隐形消耗斗气，例如刚刚准备做一个消耗斗气的动作还没有动作就立刻做出下一个动作，那么这个斗气消耗是不会返还的
		//对于上述方法暂时使用状态值来强支计算能否转换
		//  && systemValues .canInteruptActionInAttack ( this.thePlayer .theAnimationController)  ) 
		//上面的检查方法被移动到了attackLinkController里面，在attackLink里面只管实现攻击效果，不再判断是否可以转移攻击效果（多次分时的检查可能会出现卡顿）
		if (string.IsNullOrEmpty (animationName) == false )
		{
			thePlayer.theAudioPlayer.playAttackActSound (this.audioWhenAct);//播放攻击动作音效
			if (thePlayer.ActerSp >= this.spUse)
			{
				thePlayer.ActerSp -= this.spUse;
			} 
			else 
			{
				//法力透支的计算过程
				float hpMinus = this.spUse - thePlayer.ActerSp;
				thePlayer.ActerHp -= hpMinus*1.2f;
				thePlayer.ActerSp = 0;
				if (thePlayer.ActerHp < 10)
					thePlayer.ActerHp = 10f;//保护机制，在格斗游戏中没有透支身亡一说
				//但是为了保证我的个性，透支机制依旧存在，只是不会致命了
			}
			thePlayer.OnUseSp (this.spUse);

			switch( crossMode )
			{
			//平滑过渡
			case crossFadeMode.crossfade :
				{
				     this.theAnimatorOfPlayer.CrossFade (animationName, 0.05f);
				}
				break;
			//一般播放
			case crossFadeMode.play :
				{
					this.theAnimatorOfPlayer.Play (animationName);
				}
				break;
			//强制播放(不太推荐的做法)
		    //其实强制播放也没用，在这上一层会有检查
			case crossFadeMode.crossfadeFix :
				{
					this.theAnimatorOfPlayer.CrossFadeInFixedTime(animationName, 0.00f);
				}
				break;

			}

			thePlayer.extraDamageForAnimation = this.extraDamage;//用这样的方式修改真正的伤害
			thePlayer .theAudioPlayer .audioNow = this.audioWhenAttack;//确定命中的时候的音效

			thePlayer.conNameToEMY = this.conNameToEMY;
			thePlayer.conNameToSELF = this.conNameToSELF;
		}

		//print (skillName+" 发动！");
	}


	public char getCharWithIndex(int index)
	{
		if (attackAray.Length-1 < index)
			return ' ';
		else
		 return attackAray [index];//返回当前检测的字符
	}
	//初始化
	void Start () 
	{
		makeStart();//初始化ARRAY，这个是为了方便输入，并且在Unity中编辑字符串要比char数组方便

	}

	public string getAttackLinkName()//返回连招名称
	{
		string information = "";
		if (string.IsNullOrEmpty (this.conNameToEMY) == false || string.IsNullOrEmpty (this.conNameToSELF) == false)
			information += systemValues .hasSkillColor;
		else
			information += systemValues .normalAttackLinkColor;
		information += this.skillName+ systemValues .colorEnd + "\n";

		return information;
	}

	public string getInformation()//获取连招的信息
	{

		string information = "";

		information += "触发方式：" + systemValues .getattackLinkChinese(this.attackLinkString)+"\n";
		information += "额外伤害：" + this.extraDamage+"\n";
		information += "斗气消耗：" + this.spUse;
		return information;
	}
	//更新
	void Update () {
		
	}
}
