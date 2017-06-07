
function SetValueWithTag(datetext,alertStr)
{
	alert(alertStr);
	
	//var datetext = document.getElementById(dataTextID);
	
	//���tag��Ϣ����ȡ��ֵ����������tagֵ
	//alert(datetext.tag);
	//alert(IsDateTime(datetext.tag));
	if(IsDateTime(datetext.tag))
	{
		//alert("is date");
		var oldtag = datetext.tag;
		newvalue =oldtag.substring(0,4)+oldtag.substring(5,7)+oldtag.substring(8,10);
		datetext.value = newvalue;	
		//alert(datetext.value);							
	}
	else
	{
		datetext.value = "";
		datetext.tag = "";		
	}
	//alert(datetext.value);
	datetext.select();
	//���tag�����ڸ�ʽ����ʹ��tagֵ���ã��������Text��Ϣ
	
}


function OutDateText(dataTextID,alertStr){			
			//alert("out id = "+dataTextID);
			var datetext = document.getElementById(dataTextID);

			try
			{
				//replace empty string in dateTime string of start and end
				var oldvalue = datetext.value;
				var newvalue = "";
				sre = /^\s*/;
				oldvalue = oldvalue.replace(sre,"");				
				ere = /\s*$/;
				oldvalue = oldvalue.replace(ere,"");
				//alert("out oldvalue =" +oldvalue);
				//if dateTime string not is empty string,check format		
				//����������������ʽ���������text		
				if (oldvalue.length != 0)
				{ 
					//alert("value =" +  oldvalue);
					//alert("tag =" +  datetext.tag);
					//����������10���ַ�����һ������ȡtag����Ϣ
					//alert(oldvalue.length);
					if (oldvalue.length > 10)
					{
						SetValueWithTag(datetext,alertStr)
						//alert(datetext.value);
					}
					else
					{
						//�����yyyy-mm-dd�����ڸ�ʽ����������ֵ��������Ҫת��Ϊyyyy-mm-dd��ʽ����
						//alert(IsDateTime(oldvalue));
						if(IsDateTime(oldvalue))
						{	
							//alert("set old st ");						
							datetext.value = oldvalue;
							datetext.tag = oldvalue;	
							//alert("set old ");						
						}
						else
						{
							//alert("oldvalue = " + oldvalue);
							//�������8���ַ����ʾ���ǺϷ����ڣ�ʹ��tag��ֵ����
							if (oldvalue.length==8)
							{
								//ת��yyyymmdd��ʽΪyyyy-mm-dd
								newvalue = oldvalue.substring(0,4)+"-"+oldvalue.substring(4,6)+"-"+oldvalue.substring(6,8)
								//alert("newvalue = "+newvalue);
								//alert(IsDateTime(newvalue));
								//����������ڸ�ʽ����ʹ��tagֵ���ã�����ʹ��ת�����ֵ����
								if (IsDateTime(newvalue))
								{										
									datetext.tag = newvalue;
									datetext.value = newvalue;
								}
								else
								{
									SetValueWithTag(datetext,alertStr)
								}
								//alert("ok");
							}
							else
							{
								SetValueWithTag(datetext,alertStr)
							}
						}
					}
				}
				else
				{
					//������
					//alert("kong")
					datetext.value="";
					datetext.tag="";					
					//newvalue = oldvalue.substring(0,4)+"-"+oldvalue.substring(4,6)+"-"+oldvalue.substring(6,8)
				}		
				//alert("end value =" + datetext.value);		
			}
			catch(e)
			{
				alert(e);	
				alert("e");			
				datetext.value = "";
				datetext.tag = "";
				//datetext.select();
			}
		}
		
		//Getfocus and Start input dateTime string
		function InDateText(dataTextID) {	
							
			//alert("in id = "+dataTextID);	
			var datetext = document.getElementById(dataTextID);

			try
			{
				//replace empty string in dateTime string of start and end
				var oldvalue = datetext.value;
				var newvalue = "";
				sre = /^\s*/;
				oldvalue = oldvalue.replace(sre,"");				
				ere = /\s*$/;
				oldvalue = oldvalue.replace(ere,"");
				
				//if dateTime string not is empty string,check format				
				if (oldvalue.length != 0)
				{
					//alert("in oldvalue = " + oldvalue);
					//check datetime string format
					if (IsDateTime(oldvalue))
					{
						newvalue = oldvalue.substring(0,4)+oldvalue.substring(5,7)+oldvalue.substring(8,10);
					}
					else
					{
						oldvalue = "";
					}
					
				}
				
				//save oldvalue in tag and set datetext's value
				datetext.tag = oldvalue;
				datetext.value = newvalue;
				datetext.select();
			}
			catch(e)
			{
				alert(e);		
				datetext.tag = "";
				datetext.value = "";
				//datetext.select();
			}			
		}
		
		function IsDateTime(strValue)
		{
			//check strValue format is datetime  
			strValue=new String(strValue);
			var Reg;
			var iDotPos,iYear,strRight,iMonth,iDay;

			//reg of datatime��d(n) denotation "(n)number"��"|" denotation "and"����-" is list separator��
			//2004-12-12 is ok

			Reg=/\d{4}-\d{2}-\d{2}/;   
			
			//alert(strValue);
			
			if(Reg.test(strValue))           //test reg is ok
			{
				iDotPos=strValue.search("-");
				iYear=strValue.substring(0,iDotPos);    //read year
				
				maxYear = 10000;				
				minYear = 1000;
				
				//alert(iYear);
				
				if(iYear>maxYear || iYear<minYear)
				{
					return false;
				}				

				strRight=strValue.substring(iDotPos+1,strValue.length);
				iDotPos=strRight.search("-");
				iMonth=strRight.substring(0,iDotPos);        //read month

				iDay=strRight.substring(iDotPos+1,strRight.length);   //read date
				
				//alert(iMonth);
				//alert(iDay);

				if(iMonth>12||iMonth==0)     
				{
					return false;
				}
				
			

				if(iMonth==2)
				{
					if (iYear<100) iYear+=2000;
					if ((iYear % 4 == 0 && iYear % 100 != 0) || iYear % 400 == 0)    //if leap year 
					{
						if(iDay>29||iDay==0) return false; 
					}
					else
					{
						if(iDay>28||iDay==0) return false;
					}
					return true;
				}
				else if(iMonth==1 || iMonth==3 || iMonth == 5 ||iMonth==7 || iMonth==8 || iMonth==10 || iMonth==12) 
				{
					if(iDay>31||iDay==0)
					{
						return false;
					}
				}
				else
				{
					if(iDay>30||iDay==0)
					{
						return false;
					}
				}		       

				return true;  
			}
			else
			{  
				return false;
			}
		}
		
		function num_press()
		{
			//alert(event.keyCode);
			if(event.keyCode>=48 && event.keyCode<=57 || event.keyCode==8 )
			{
				return true;
			}
			else
			{
				//alert(event.keyCode);
				event.keyCode=0
				return false
			}
		}