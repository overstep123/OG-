using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OG
{
    public partial class Form1 : Form
    {
        //优先关系矩阵
        int[,] re={ 
            { 1, -1, -1, -1, 1, 1},
            { 1,  1, -1, -1, 1, 1},
            { 1,  1,  2,  2, 1, 1},
            { -1,-1, -1, -1, 0, 2},
            { 1,  1,  2,  2, 1, 1},
            { -1, -1,-1, -1, 2, 0}
        };
        String st;
        int p;
        int j;
        int h;
        char a;
        String s;

        public Form1()
        {
            InitializeComponent();
        }
        public void init(int n, String r)
        {
            st = "#";
            p = 0;
            s = r;
            h = 0;
        }
        public int num(char c)
        {
            switch(c)
            {
                case '+':
                    return 0;
                case '*':
                    return 1;
                case 'i':
                    return 2;
                case '(':
                    return 3;
                case ')':
                    return 4;
                case '#':
                    return 5;
                default:
                    return -1;
            }
        }
        public int jude(String r,int i)
        {
            if (p == 1 && st[p] == '#' && (r[i]== '+' || r[i] == '*'))
            {
                return 0; 
            }
            if ((r[i] == '+'|| r[i] == '*')&&(r[i-1]=='+'||r[i-1]=='*'))
            {
                return 0;
            }
            if (r[i] == '#' && (r[i-1] == '+' || r[i-1] == '*'))
            {
                return 0;
            }
            return 1;
        }
        public void show(int r,char g,int d,int b)//r为优先关系值，g为当前符号，d=0表示移进，d=1表示规约；b=0表示分析失败，b=1表示分析继续，b=2表示分析成功
        {
            ListViewItem word = new ListViewItem();
            word.Text = (++h).ToString();
            word.SubItems.Add(st);
                if (r == -1)
                    word.SubItems.Add("<");
                else if (r == 1) word.SubItems.Add(">");
                else if(r==0) word.SubItems.Add("=");
                else word.SubItems.Add("error");
                word.SubItems.Add(g.ToString());
                word.SubItems.Add(s);
            if (b == 1)
            {
                if (d == 0)
                    word.SubItems.Add("移进");
                else word.SubItems.Add("规约");
            }
            else if(b==0)
                word.SubItems.Add("分析失败！");
            else if(b==2)
                word.SubItems.Add("分析成功！");
            listView1.Items.Add(word);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            int relation;
            int i = 0;
            String r = textBox1.Text;
            if(r.Length<=0)
            {
                MessageBox.Show("input blank!");
            }
            else
            {
                init(r.Length, r);
                while(1>0)
                {
                    a = r[i];
                    if (jude(r,i)==0)
                    {
                        show(2, a, 1, 0);
                        break;
                    }
                    if (num(st[p]) != -1) j = p;
                    else j = p - 1;
                    relation = re[num(st[j]), num(a)];
                    if (relation == -1)
                    {
                        s = r.Substring(i + 1);
                        show(relation, a, 0,1);
                        p++;
                        st += a.ToString();
                        i++;
                        continue;
                    }
                    else if(relation ==0)
                    {
                        if (st[j] == '#')
                        {
                            show(relation, a, 0, 2);
                            break;
                        }
                        else
                        {
                            s = r.Substring(i + 1);
                            show(relation, a, 0, 1);
                            p++;
                            st += a.ToString();
                            i++;
                            continue;
                        }
                    }
                    else if(relation==1)
                    {
                        char q=st[j];
                        if (num(st[j - 1])!=-1)
                            j--;
                        else
                            j = j - 2;
                        while (re[num(st[j]),num(q)]!=-1)
                        {
                            q = st[j];
                            if (num(st[j - 1]) != -1)
                                j--;
                            else
                                j = j - 2;
                        }
                        s = r.Substring(i + 1);
                        show(relation, a, 1, 1);
                        p = j + 1;
                        if (st[p] == '(' || st[p] == 'i')
                            st = st.Substring(0, p) + "F";
                        else if (st[p+1] == '*')
                            st = st.Substring(0, p) + "T";
                        else st = st.Substring(0, p) + "E";
                        continue;
                    }
                    else
                    {
                        show(2, a, 1, 0);
                        break;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = null;
            listView1.Items.Clear();
        }
    }
}
