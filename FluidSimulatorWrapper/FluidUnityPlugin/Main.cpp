#include "Main.h"
#include "sph_header.h"
#include "sph_data.h"
//#include "sph_timer.h"
#include "sph_system.h"

SPHSystem *sph;


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

extern "C" EXPORT_API bool InitVariableSystem(int initialParticles,int maxParticles,float kernelInput, float massInput, float gravX, float gravY, float gravZ, float worldSizeX, float worldSizeY,float worldSizeZ,float wallDampening, float restDencity,float gasConstant, float viscosityInput, float timeStep, float surfaceNormals, float surfaceCoeffeciant)
{
    
    real_world_origin.x=-10.0f;
    real_world_origin.y=-10.0f;
    real_world_origin.z=-10.0f;
    
    real_world_side.x=20.0f;
    real_world_side.y=20.0f;
    real_world_side.z=20.0f;
    
    sph = new SPHSystem(initialParticles, maxParticles,kernelInput,massInput,gravX,gravY, gravZ,  worldSizeX,  worldSizeY, worldSizeZ, wallDampening,  restDencity, gasConstant,  viscosityInput,  timeStep,  surfaceNormals,  surfaceCoeffeciant);
    sph->send_callback("System Initialised");
    sph->init_limited_system();
    
    if (sph != nullptr && sph->sys_running == 0) {
        return true;
    }else
    {
        return false;
    }

}

extern "C" EXPORT_API void AddParticle(float PosX, float PosY, float PosZ, float VelX, float VelY, float VelZ )
{
    float3 pos;
    float3 vel;
    if(sph != nullptr)
    {
        pos.x = PosX;
        pos.y = PosY;
        pos.z = PosZ;
        
        vel.x = VelX;
        vel.y = VelY;
        vel.z = VelZ;
        
        sph->add_particle(pos, vel);
    }
}

extern "C" EXPORT_API void GetInternalPoints(float* arrayin, int height, int width , float worldRatio)
{
    sim_ratio.x=real_world_side.x/sph->world_size.x;
    sim_ratio.y=real_world_side.y/sph->world_size.y;
    sim_ratio.z=real_world_side.z/sph->world_size.z;
    
    
    for (int i = 0; i<height; i++) {
        
        arrayin[i*width] = sph->mem[i].pos.x * worldRatio;  //* sim_ratio.x + real_world_origin.x;
        arrayin[i*width +1] = sph->mem[i].pos.y * worldRatio; //* sim_ratio.y + real_world_origin.y;
        arrayin[i*width +2] = sph->mem[i].pos.z * worldRatio; //* sim_ratio.z + real_world_origin.z;
        
    }
    
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
