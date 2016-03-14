#include "Main.h"
#include "sph_header.h"
#include "sph_data.h"
//#include "sph_timer.h"
#include "sph_system.h"

SPHSystem *sph;

//Timer *sph_timer;
char *window_title;

extern "C" EXPORT_API SPHSystem* InitSPHSystem(int particles)
{
    real_world_origin.x=-10.0f;
    real_world_origin.y=-10.0f;
    real_world_origin.z=-10.0f;
    
    real_world_side.x=20.0f;
    real_world_side.y=20.0f;
    real_world_side.z=20.0f;

    SPHSystem *newSph = new SPHSystem(particles);
    newSph->init_limited_system();
    return newSph;
}

extern "C" EXPORT_API SPHSystem* InitOpenSPHSystem()
{
    real_world_origin.x=-10.0f;
    real_world_origin.y=-10.0f;
    real_world_origin.z=-10.0f;
    
    real_world_side.x=20.0f;
    real_world_side.y=20.0f;
    real_world_side.z=20.0f;

    SPHSystem *newSph = new SPHSystem();
    newSph->init_system();
    //newSph->animation();
    return newSph;
}

extern "C" EXPORT_API bool InitInternalSystem()
{
    real_world_origin.x=-10.0f;
    real_world_origin.y=-10.0f;
    real_world_origin.z=-10.0f;
    
    real_world_side.x=20.0f;
    real_world_side.y=20.0f;
    real_world_side.z=20.0f;
    

    
    sph = new SPHSystem();
    sph->send_callback("system init called");
    sph->init_system();
    
    if (sph != nullptr && sph->sys_running == 0) {
        return true;
    }else
    {
        return false;
    }
}

extern "C" EXPORT_API bool InitVariableSystem(int maxParticles,float kernelInput, float massInput, float gravX, float gravY, float gravZ, float worldSizeX, float worldSizeY,float worldSizeZ,float wallDampening,float restDencity,float gasConstant, float viscosityInput, float timeStep, float surfaceNormals, float surfaceCoeffeciant, float poly6Val1, float poly6Val2,float poly6Val3, float spikyVal1,float spikyVal2,float viscoVal1, float grad6Polyval1,float grad6Polyval2,float grad6Polyval3,float viscoVal2, float lpcVal1, float lpcVal2, float lpcVal3)
{
    real_world_origin.x=-10.0f;
    real_world_origin.y=-10.0f;
    real_world_origin.z=-10.0f;
    
    real_world_side.x=20.0f;
    real_world_side.y=20.0f;
    real_world_side.z=20.0f;
    
    sph = new SPHSystem( maxParticles, kernelInput,  massInput,  gravX,  gravY,  gravZ,  worldSizeX,  worldSizeY, worldSizeZ, wallDampening, restDencity, gasConstant,  viscosityInput,  timeStep,  surfaceNormals,  surfaceCoeffeciant,  poly6Val1,  poly6Val2, poly6Val3,  spikyVal1, spikyVal2, viscoVal1,  grad6Polyval1, grad6Polyval2, grad6Polyval3, viscoVal2,  lpcVal1,  lpcVal2,  lpcVal3);
    sph->send_callback("System Initialised");
    sph->init_limited_system();
    
    if (sph != nullptr && sph->sys_running == 0) {
        return true;
    }else
    {
        return false;
    }

}

extern "C" EXPORT_API void GetInternalPoints(float* arrayin, int height, int width)
{
    sim_ratio.x=real_world_side.x/sph->world_size.x;
    sim_ratio.y=real_world_side.y/sph->world_size.y;
    sim_ratio.z=real_world_side.z/sph->world_size.z;
    
    //sph->send_callback("Sart data return");
    
    /*Particle *p;
    for (int i = 0; i<height; i++) {
        p = &(sph->mem[i]);
        
        arrayin[i*width] = p->pos.x * sim_ratio.x + real_world_origin.x;
        arrayin[i*width +1] = p->pos.y * sim_ratio.y + real_world_origin.y;
        arrayin[i*width +2] = p->pos.z * sim_ratio.z + real_world_origin.z;
        
    }*/
    
    for (int i = 0; i<height; i++) {
        
        arrayin[i*width] = sph->mem[i].pos.x * sim_ratio.x + real_world_origin.x;
        arrayin[i*width +1] = sph->mem[i].pos.y * sim_ratio.y + real_world_origin.y;
        arrayin[i*width +2] = sph->mem[i].pos.z * sim_ratio.z + real_world_origin.z;
        
    }


    //sph->send_callback("Look I am being called");
    
}

extern "C" EXPORT_API float GetInternalPoint(int id, int direction)
{
    sim_ratio.x=real_world_side.x/sph->world_size.x;
    sim_ratio.y=real_world_side.y/sph->world_size.y;
    sim_ratio.z=real_world_side.z/sph->world_size.z;
    
    float value = 0;
    if (id < sph->num_particle) {
        Particle *p = &(sph->mem[id]);
        switch (direction) {
            case 0:
                value = p->pos.x * sim_ratio.x + real_world_origin.x;
                break;
            case 1:
                value = p->pos.y * sim_ratio.y + real_world_origin.y;
                break;
            case 2:
                value = p->pos.z * sim_ratio.z + real_world_origin.z;
                break;
            default:
                value = 0;
                break;
        }
    }
    
    return value;
    
}

extern "C" EXPORT_API void InternalAnimate()
{
    sph->animation();
}

extern "C" EXPORT_API void InternalDispose()
{
    if (sph != NULL)
    {
        delete sph;
        sph = NULL;
    }
}

extern "C" EXPORT_API int GetInternalLength()
{
    return (int)sph->num_particle;
}

extern "C" EXPORT_API void InternalStartRunning()
{
    sph->sys_running=1-sph->sys_running;
}

extern "C" EXPORT_API int InternalRunningState()
{
    return sph->sys_running;
}

extern "C" EXPORT_API void DisposeSPHSystem(SPHSystem* pObject)
{
	if (pObject != NULL)
	{
		delete pObject;
		pObject = NULL;
	}
}

extern "C" EXPORT_API SPHSystem* Animate(SPHSystem* pObject)
{ 
	pObject->animation();
	return pObject;
}

extern "C" EXPORT_API void Animation(SPHSystem* pObject)
{
    pObject->animation();
}

extern "C" EXPORT_API SPHSystem* Animate1(SPHSystem* pObject)
{
    SPHSystem* tmpSystem = pObject;
    tmpSystem->animation();
    return tmpSystem;
}

extern "C" EXPORT_API void Animate2 (SPHSystem* pObject, double* arrayin, int height, int width)
{
    pObject->animation();
    Particle *p;
    Particle *mem = pObject->mem;
    for (int i = 0; i<height; i++) {
        p = &(mem[i]);
        
        arrayin[i*width] = (double)(p->pos.x * sim_ratio.x + real_world_origin.x) + 1;
        arrayin[i*width +1] = (double)p->pos.y * sim_ratio.y + real_world_origin.y;
        arrayin[i*width +2] = (double)p->pos.z * sim_ratio.z + real_world_origin.z;
        
    }
}

extern "C" EXPORT_API int GetLength(SPHSystem* pObject)
{
    return (int)pObject->num_particle;
}

extern "C" EXPORT_API void GetPoints(SPHSystem* pObject, float* arrayin, int height, int width)
{
    sim_ratio.x=real_world_side.x/pObject->world_size.x;
    sim_ratio.y=real_world_side.y/pObject->world_size.y;
    sim_ratio.z=real_world_side.z/pObject->world_size.z;
    
    Particle *p;
    Particle *mem = pObject->mem;
    for (int i = 0; i<height; i++) {
        p = &(mem[i]);
        
        arrayin[i*width] = (p->pos.x * sim_ratio.x + real_world_origin.x) + 1;
        arrayin[i*width +1] = p->pos.y * sim_ratio.y + real_world_origin.y;
        arrayin[i*width +2] = p->pos.z * sim_ratio.z + real_world_origin.z;
        
    }
}


extern "C" EXPORT_API void ChangePoints(SPHSystem* pObject, double* arrayin, int height, int width)
{
    for (int i = 0; i<height; i++) {
        for (int j  =0 ; j <width; j++) {
            arrayin[i*width +j] = arrayin[i*width +j]+100;
        }
    }
}

extern "C" EXPORT_API int GetRunState(SPHSystem* pObject)
{
    return pObject->sys_running;
}

extern "C" EXPORT_API void StartRunning(SPHSystem* pObject)
{
    pObject->sys_running=1-pObject->sys_running;
}

void init_sph_system()
{
	real_world_origin.x=-10.0f;
	real_world_origin.y=-10.0f;
	real_world_origin.z=-10.0f;

	real_world_side.x=20.0f;
	real_world_side.y=20.0f;
	real_world_side.z=20.0f;

	sph=new SPHSystem();
	sph->init_system();

	//sph_timer=new Timer();
	window_title=(char *)malloc(sizeof(char)*50);
}


void init_ratio()
{
	sim_ratio.x=real_world_side.x/sph->world_size.x;
	sim_ratio.y=real_world_side.y/sph->world_size.y;
	sim_ratio.z=real_world_side.z/sph->world_size.z;
}

void render_particles()
{
	for(uint i=0; i<sph->num_particle; i++)
	{
	}
}

void display_func()
{
	sph->animation();
	render_particles();
}

int main()
{
    
}
