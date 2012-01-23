// BuldyrevPort.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#define M_PI (3.14159261)

#include <stdio.h> //include standard input-output library
#include <math.h>  //include math library (we do not need it in this particular program)
#include <stdlib.h> //include standard library for memory allocations
#include <float.h>  //include library for finding out the limits of floating point values
#include <string.h> //include library for string manipulation .e.g. to call strcat()



void write_bmp_header(int lx, int ly, FILE * ff)
{
  int bmp[13];
  double a,b,z,z1;
  int i,j,k,l;
  unsigned char color[2];
  color[0]='B';
  color[1]='M';
  fwrite(&color[0],1,2,ff);
  for(i=0;i<13;i++)
	bmp[i]=0;
  bmp[0]=lx*ly*3+54;
  bmp[2]=54;
  bmp[3]=40;
  bmp[4]=lx;
  bmp[5]=ly;
  bmp[6]=1572865;
  bmp[8]=lx*ly*3;
  /*bmp[9]=2834;
	bmp[10]=2834;*/
  fwrite(&bmp[0],4,13,ff);
}
unsigned char * make_color1(int * ncol1)
{
  int ncol=5*255,k,j,l;
  unsigned char *color;
  color=(unsigned char *)malloc(3*(ncol+1)*sizeof(unsigned char));
  for(k=1;k<=ncol;k++)
	{
	  j=(ncol-k+1)/255;
	  l=(ncol-k+1)%255;
	  switch (j)
	{
	case 0:
	color[k*3+2]=255;
		color[k*3+1]=l; 
	color[k*3]=0;
	break;
	case 1:
	color[k*3+2]=255-l;
		color[k*3+1]=255; 
	color[k*3]=0;
	break;
	case 2:
	color[k*3+2]=0;
		color[k*3+1]=255; 
	color[k*3]=l;
	break;
	case 3:
	color[k*3+2]=0;
		color[k*3+1]=255-l; 
	color[k*3]=255;
	break;
	case 4:
	color[k*3+2]=l;
		color[k*3+1]=0; 
	color[k*3]=255;
	break;
	}
	}
  color[0]=0;
  color[1]=0;
  color[2]=0;
  *ncol1=ncol;
  return color;
}


int randomWalker()//main function in this program this is the only function used
{
	
	//Passed Parameters:
	char fname[1000] = "testing.txt";
	double dx =10; 
	double dx2 = 10; 
	double dz =10; 
	double dz1 =10; 
	double r = 10; 
	double dr = 1; 
	double alpha = 1; 
	int k1 = 5; 
	int k2 =5 ; 
	int nrun = 100;
	unsigned int rn = 10; 
	int nt = 1; 
	int ts = 1; 
	int nbin = 1; 
	double bin = 1; 
	//magnification
	int mm =1; 
	int transp[3];
	for(int i=0; i < 3;i++){
		transp[i] = i+ 1;
	}

	//****
  double x,y,z,x1,y1,z1,x2,y2,z2,drx,dry,drz,phi,cth,sth,dr1,xt,yt,zt;
  double ca,sa,rr;
  double step,step1;
  double cx1[4],cz1[4],cx2[4],cz2[4];
  int ix[3];
  int i,j,k,l,m,n,n2,n3,yes;
  int maxt;
  int ns;
  int tc,t0;
  double bin1;
  double half;
  int *count,*count1;
  int *dt;
  int first=1;
  double *x0,*y0,*z0;
  
  int irun;
  FILE *ff; //output file
  char fname1[1000] = "output.txt"; // name of the output file
  unsigned char * color;
  unsigned char *color1;
  double fact;
  int ncol,mm1,mm2,l1,l2;
  maxt=ts*nt*ts*nt;
  n=2*nbin;
  half=nbin*bin;
  bin1=1/bin;
  n2=n*n;
  n3=n*n2;
  ns=n3*nt;
  count=(int *)malloc(ns*sizeof(int));
  dt=(int *)malloc(nt*sizeof(int));
  for(i=0;i<ns;i++)
	count[i]=0;
  x0=(double*)malloc((maxt+1)*sizeof(double));
  y0=(double*)malloc((maxt+1)*sizeof(double));
  z0=(double*)malloc((maxt+1)*sizeof(double));
  for(i=1;i<=nt;i++)
	dt[i-1]=ts*i*ts*i;
  srand(rn);
  step=(k1+k2-2)*dz+2*dz1;
  step1=(k1-1)*dz+dz1;
  z1=0;
  x1=dx*0.5;
  y1=0;
  xt=0;
  yt=0;
  zt=0;
  x0[0]=0;
  y0[0]=0;
  z0[0]=0;
  tc=0;
  rr=r*r;
  ca=cos(M_PI*alpha/180);
  sa=sin(M_PI*alpha/180);
  x2=x1*ca+y1*sa;
  y2=-x1*sa+y1*ca;
 for(irun=0;irun<nrun;irun++) //initialization of the invasion front
   { 
	 cth=2*rand()-1;
	 sth=sqrt(1-cth*cth);
	 phi=M_PI*2*rand();
	 dr1=dr*rand();
	 drx=dr1*sth*cos(phi);
	 dry=dr1*sth*sin(phi);
	 drz=dr1*cth;
	 x=x1+drx;
	 y=y1+dry;
	 z=z1+drz;
	 z2=z;
	 if(z2>=step)z2-=step;
	 if(z2<0)z2+=step;
	 if(z2<step1-dz1)
	   {
	 z=z2;
	 i=floor(z/dz);
	 z-=i*dz;
	 x-=i*dx2;
	 j=floor(x/dx);
	 x-=j*dx;
	 for(k=0;k<4;k++)
	   {
		 cx2[k]=x;
		 cz2[k]=z;
	   }
	 cz1[0]=0;
	 cz1[1]=0;
	 cz1[2]=dz;
	 cz1[3]=dz;
	 cx1[0]=0;
	 cx1[1]=dx;
	 cx1[2]=dx2;
	 if(x<dx2)
	   cx1[3]=dx2-dx;
	 else
	   cx1[3]=dx2+dx;
	   }
	 else if(z2<step1)
	   {
	 z=z2-(k1-1)*dz;
	 x2=x;
	 x-=(k1-1)*dx2;
	 j=floor(x/dx);
	 x-=j*dx;
	 for(k=0;k<2;k++)
	   {
		 cx2[k]=x;
		 cz2[k]=z;
		 cz1[k]=0;
	   }
	 cx1[0]=0;
	 cx1[2]=dx;
	 x=x2*ca+y*sa;
	 j=floor(x/dx);
	 x-=j*dx;
	 for(k=2;k<4;k++)
	   {
		 cx2[k]=x;
		 cz2[k]=z;
		 cz1[k]=dz1;
	   }
	 cx1[2]=0;
	 cx1[3]=dx;
	   }
	 else if(z2<step-dz1)
	   {
	 z=z2-step1;
	 x=x*ca+y*sa;
	 i=floor(z/dz);
	 z-=i*dz;
	 x-=i*dx2;
	 j=floor(x/dx);
	 x-=j*dx;
	 for(k=0;k<4;k++)
	   {
		 cx2[k]=x;
		 cz2[k]=z;
	   }
	 cz1[0]=0;
	 cz1[1]=0;
	 cz1[2]=dz;
	 cz1[3]=dz;
	 cx1[0]=0;
	 cx1[1]=dx;
	 cx1[2]=dx2;
	 if(x<dx2)
	   cx1[3]=dx2-dx;
	 else
	   cx1[3]=dx2+dx;
	   }
	 else
	   {
	 z=z2-step1;
	 z-=(k2-1)*dz;
	 x2=x;
	 x=x2*ca+y*sa;
	 x-=(k2-1)*dx2;
	 j=floor(x/dx);
	 x-=j*dx;
	 for(k=0;k<2;k++)
	   {
		 cx2[k]=x;
		 cz2[k]=z;
		 cz1[k]=0;
	   }
	 cx1[0]=0;
	 cx1[2]=dx;
	 x=x2;
	 j=floor(x/dx);
	 x-=j*dx;
	 for(k=2;k<4;k++)
	   {
		 cx2[k]=x;
		 cz2[k]=z;
		 cz1[k]=dz1;
	   }
	 cx1[2]=0;
	 cx1[3]=dx;
	   }
	 yes=1;
	 for(k=0;k<4;k++)
	   {
	 x=cx1[k]-cx2[k];
	 z=cz1[k]-cz2[k];
	 if(x*x+z*z<rr){yes=0;break;}
	   }
	 if(yes)
	 {
	   x1+=drx;
	   y1+=dry;
	   z1=z2;
	   xt+=drx;
	   yt+=dry;
	   zt+=drz;
	 }
	 tc++;
	 if(tc==maxt)
	   {
	 first=0;
	 tc=0;
	   }
	   for(i=0;i<nt;i++)
	 {
	   t0=tc-dt[i];
	   if(t0<0)
		 {
		   if(first)break;
		   t0+=maxt;
		 }
	   x=xt-x0[t0]+half;
	   y=yt-y0[t0]+half;
	   z=zt-z0[t0]+half;
	   ix[transp[0]]=floor(x*bin1);
	   ix[transp[1]]=floor(y*bin1);
	   ix[transp[2]]=floor(z*bin1);
	   yes=1;
	   for(j=0;j<3;j++)
		 {
		   if(ix[j]<0){yes=0;break;}
		   if(ix[j]>n){yes=0;break;}
		 }
	   if(yes)
		 {
		   k=0;
		   for(j=2;j>=0;j--)
		 {
		   k=k*n+ix[j];
		 }
		   k+=i*n3;
		   count[k]++;
		 }
	 }
	 x0[tc]=xt;
	 y0[tc]=yt;
	 z0[tc]=zt;
   }
 color=make_color1(&ncol);
 for(i=0;i<nt;i++)
   {
	 int m1;
	 double fact1;
	 k=0;
	 count1=count+n3*i;
	 for(j=0;j<n3;j++)
	   if(count1[j]>k)k=count1[j];
	 fact=ncol/(double)k;
	 for(j=0;j<n;j++)
	   {
	 count1=count+(n3*i+n2*j);
	 sprintf(fname1,"%s-%04d-%04d.bmp",fname,i,j);
	 ff=fopen(fname1,"wb");
	 write_bmp_header( n*mm, n*mm, ff);
	 for(l1=0;l1<n;l1++)
	   for(mm1=0;mm1<mm;mm1++)
		 for(l2=0;l2<n;l2++)
		   {
		 l=l2+l1*n;
		 m=floor(count1[l]*fact);
		 for(mm2=0;mm2<mm;mm2++)
		   fwrite(&color[m*3],1,3,ff);
		   }
	 fclose(ff);
	}
  }

 return 1;
}

int _tmain(int argc, _TCHAR* argv[])
{
	randomWalker();
	return 0;
}

