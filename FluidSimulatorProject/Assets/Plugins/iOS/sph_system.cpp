

#include "sph_system.h"
#include "sph_header.h"

typedef void (*FuncPtr)( const char * );

FuncPtr Debug;

extern "C" EXPORT_API void SetDebugFunction( FuncPtr fp )
{
    Debug = fp;
}

void SPHSystem::send_callback(std::string message)
{
    //Debug(message.c_str());
}

SPHSystem::SPHSystem()
{
    init_particle = 11600;
    send_callback("Initial Particles = " + float_conversion(init_particle));
	max_particle=30000;
    send_callback("Max Particles = " + float_conversion(max_particle));
	num_particle=0;
    send_callback("Initial Particles = " + float_conversion(num_particle));

	kernel=0.04f;
    send_callback("Kernal = " + float_conversion(kernel));
	mass=0.02f;
    send_callback("Mass = " + float_conversion(mass));

	world_size.x=0.64f;
    send_callback("World Size X = " + float_conversion(world_size.x));
	world_size.y=0.64f;
    send_callback("World Size Y = " + float_conversion(world_size.y));
	world_size.z=0.64f;
    send_callback("World Size Z = " + float_conversion(world_size.z));
	cell_size=kernel;
    send_callback("Cell Size = " + float_conversion(cell_size));
	grid_size.x=(uint)ceil(world_size.x/cell_size);
    send_callback("Grid Size X = " + float_conversion((float)grid_size.x));
	grid_size.y=(uint)ceil(world_size.y/cell_size);
    send_callback("Grid Size Y = " + float_conversion((float)grid_size.y));
	grid_size.z=(uint)ceil(world_size.z/cell_size);
    send_callback("Grid Size Z = " + float_conversion((float)grid_size.z));
	tot_cell=grid_size.x*grid_size.y*grid_size.z;
    send_callback("Total Cells = " + float_conversion(tot_cell));

	gravity.x=0.0f;
    send_callback("Gravity X = " + float_conversion(gravity.x));
	gravity.y=6.8f;
    send_callback("Gravity Y = " + float_conversion(gravity.y));
	gravity.z=0.0f;
    send_callback("Gravity Z = " + float_conversion(gravity.z));
	wall_damping=-0.5f;
    send_callback("Wall Dampening = " + float_conversion(wall_damping));
	rest_density=1000.0f;
    send_callback("Rest Density = " + float_conversion(rest_density));
	gas_constant=1.0f;
    send_callback("Gas Constant = " + float_conversion(gas_constant));
	viscosity=6.5f;
    send_callback("Viscosity = " + float_conversion(viscosity));
	time_step=0.003f;
    send_callback("Time Step = " + float_conversion(time_step));
	surf_norm=6.0f;
    send_callback("Surface normals = " + float_conversion(surf_norm));
	surf_coe=0.1f;
    send_callback("Surface coeffeciant= " + float_conversion(surf_coe));

	poly6_value=315.0f/(64.0f * PI * pow(kernel, 9));;
    send_callback("Poly6 value = " + float_conversion(poly6_value));
	spiky_value=-45.0f/(PI * pow(kernel, 6));
    send_callback("Spiky value = " + float_conversion(spiky_value));
	visco_value=45.0f/(PI * pow(kernel, 6));
    send_callback("Viscosity Value = " + float_conversion(visco_value));

	grad_poly6=-945/(32 * PI * pow(kernel, 9));
    send_callback("Grad Poly 6 = " + float_conversion(grad_poly6));
	lplc_poly6=-945/(8 * PI * pow(kernel, 9));
    send_callback("Lplc Poly 6 = " + float_conversion(lplc_poly6));

	kernel_2=kernel*kernel;
    send_callback("Kernal Squared = " + float_conversion(kernel_2));
	self_dens=mass*poly6_value*pow(kernel, 6);
    send_callback("Self Dencity = " + float_conversion(self_dens));
	self_lplc_color=lplc_poly6*mass*kernel_2*(0-3/4*kernel_2);
    send_callback("Lplc colour = " + float_conversion(self_lplc_color));

	mem=(Particle *)malloc(sizeof(Particle)*max_particle);
	cell=(Particle **)malloc(sizeof(Particle *)*tot_cell);

	sys_running=0;
}

SPHSystem::SPHSystem(int initialParticles, int maxParticles,float kernelInput, float massInput, float gravX, float gravY, float gravZ, float worldSizeX, float worldSizeY,float worldSizeZ,float wallDampening, float restDencity,float gasConstant, float viscosityInput, float timeStep, float surfaceNormals, float surfaceCoeffeciant)
{
    init_particle = (uint)initialParticles;
    send_callback("Initial Particles = " + float_conversion(init_particle));
    max_particle=(uint)maxParticles;
    send_callback("Max Particles = " + float_conversion(max_particle));
    num_particle=0;
    send_callback("Initial Particles = " + float_conversion(num_particle));
    
    kernel=kernelInput;
    send_callback("Kernal = " + float_conversion(kernel));
    mass=massInput;
    send_callback("Mass = " + float_conversion(mass));
    
    world_size.x=worldSizeX;
    send_callback("World Size X = " + float_conversion(world_size.x));
    world_size.y=worldSizeY;
    send_callback("World Size Y = " + float_conversion(world_size.y));
    world_size.z=worldSizeZ;
    send_callback("World Size Z = " + float_conversion(world_size.z));
    cell_size=kernel;
    send_callback("Cell Size = " + float_conversion(cell_size));
    grid_size.x=(uint)ceil(world_size.x/cell_size);
    send_callback("Grid Size X = " + float_conversion((float)grid_size.x));
    grid_size.y=(uint)ceil(world_size.y/cell_size);
    send_callback("Grid Size Y = " + float_conversion((float)grid_size.y));
    grid_size.z=(uint)ceil(world_size.z/cell_size);
    send_callback("Grid Size Z = " + float_conversion((float)grid_size.z));
    tot_cell=grid_size.x*grid_size.y*grid_size.z;
    send_callback("Total Cells = " + float_conversion(tot_cell));
    
    gravity.x=gravX;
    send_callback("Gravity X = " + float_conversion(gravity.x));
    gravity.y=gravY;
    send_callback("Gravity Y = " + float_conversion(gravity.y));
    gravity.z=gravZ;
    send_callback("Gravity Z = " + float_conversion(gravity.z));
    wall_damping=wallDampening;
    send_callback("Wall Dampening = " + float_conversion(wall_damping));
    rest_density=restDencity;
    send_callback("Rest Density = " + float_conversion(rest_density));
    gas_constant=gasConstant;
    send_callback("Gas Constant = " + float_conversion(gas_constant));
    viscosity=viscosityInput;
    send_callback("Viscosity = " + float_conversion(viscosity));
    time_step=timeStep;
    send_callback("Time Step = " + float_conversion(time_step));
    surf_norm=surfaceNormals;
    send_callback("Surface normals = " + float_conversion(surf_norm));
    surf_coe=surfaceCoeffeciant;
    send_callback("Surface coeffeciant= " + float_conversion(surf_coe));
    
    poly6_value=315.0f/(64.0f * PI * pow(kernel, 9));;
    send_callback("Poly6 value = " + float_conversion(poly6_value));
    spiky_value=-45.0f/(PI * pow(kernel, 6));
    send_callback("Spiky value = " + float_conversion(spiky_value));
    visco_value=45.0f/(PI * pow(kernel, 6));
    send_callback("Viscosity Value = " + float_conversion(visco_value));
    
    grad_poly6=-945/(32 * PI * pow(kernel, 9));
    send_callback("Grad Poly 6 = " + float_conversion(grad_poly6));
    lplc_poly6=-945/(8 * PI * pow(kernel, 9));
    send_callback("Lplc Poly 6 = " + float_conversion(lplc_poly6));
    
    kernel_2=kernel*kernel;
    send_callback("Kernal Squared = " + float_conversion(kernel_2));
    self_dens=mass*poly6_value*pow(kernel, 6);
    send_callback("Self Dencity = " + float_conversion(self_dens));
    self_lplc_color=lplc_poly6*mass*kernel_2*(0-3/4*kernel_2);
    send_callback("Lplc colour = " + float_conversion(self_lplc_color));
    
    mem=(Particle *)malloc(sizeof(Particle)*max_particle);
    cell=(Particle **)malloc(sizeof(Particle *)*tot_cell);
    
    sys_running=0;

}

SPHSystem::~SPHSystem()
{
	free(mem);
	free(cell);
}

void SPHSystem::animation()
{
    //send_callback("Animation reached");
	if(sys_running == 0)
	{
		return;
	}
    
    //send_callback("Build Table reached");
	build_table();
    //send_callback("Density reached");
	comp_dens_pres();
    //send_callback("Force reached");
	comp_force_adv();
    //send_callback("Advection reached");
    advection();
    
    //send_callback("Animation Ended");
    
    //Particle* p = &mem[0];
    //send_callback("X = " + float_conversion(mem[0].pos.x * 20/world_size.x -10.0f) +
                  //" Y = " + float_conversion(mem[0].pos.y * 20/world_size.y -10.0f) +
                  //" Z = " + float_conversion(mem[0].pos.z * 20/world_size.z -10.0f));
}

void SPHSystem::init_system()
{
	float3 pos;
	float3 vel;

	vel.x=0.0f;
	vel.y=0.0f;
	vel.z=0.0f;

	for(pos.x=world_size.x*0.0f; pos.x<world_size.x*0.6f; pos.x+=(kernel*0.5f))
	{
		for(pos.y=world_size.y*0.0f; pos.y<world_size.y*0.9f; pos.y+=(kernel*0.5f))
		{
			for(pos.z=world_size.z*0.0f; pos.z<world_size.z*0.6f; pos.z+=(kernel*0.5f))
			{
				add_particle(pos, vel);
			}
		}
	}
    
	//printf("Init Particle: %u\n", num_particle);
}

void SPHSystem::init_limited_system()
{
    
    float3 pos;
    float3 vel;
    
    vel.x=0.0f;
    vel.y=0.0f;
    vel.z=0.0f;
    
    for(pos.x=world_size.x*0.0f; pos.x<world_size.x*0.6f; pos.x+=(kernel*0.5f))
    {
        for(pos.y=world_size.y*0.0f; pos.y<world_size.y*0.9f; pos.y+=(kernel*0.5f))
        {
            for(pos.z=world_size.z*0.0f; pos.z<world_size.z*0.6f; pos.z+=(kernel*0.5f))
            {
                if (num_particle<init_particle) {
                    add_particle(pos, vel);
                }
                
            }
        }
    }
}

void SPHSystem::add_particle(float3 pos, float3 vel)
{
    send_callback("Add called");
    if (pos.x<world_size.x && pos.x > 0) {
        if (pos.y<world_size.y && pos.y > 0) {
            if (pos.z<world_size.z && pos.z > 0) {
                if(num_particle < max_particle)
                {
                    Particle *p=&(mem[num_particle]);
                    
                    p->id=num_particle;
                    
                    p->pos=pos;
                    p->vel=vel;
                    
                    p->acc.x=0.0f;
                    p->acc.y=0.0f;
                    p->acc.z=0.0f;
                    p->ev.x=0.0f;
                    p->ev.y=0.0f;
                    p->ev.z=0.0f;
                    
                    p->dens=rest_density;
                    p->pres=0.0f;
                    
                    p->next=NULL;
                    
                    num_particle++;
                    send_callback("Particle Added");
                }

            }
        }
    }
}

void SPHSystem::build_table()
{
    //send_callback("build Table started");
    Particle *p;
    uint hash;
    
    for(uint i=0; i<tot_cell; i++)
    {
        cell[i]=NULL;
    }
    
    for(uint i=0; i<num_particle; i++)
    {
        p=&(mem[i]);
        hash=calc_cell_hash(calc_cell_pos(p->pos));
        
        if(cell[hash] == NULL)
        {
            p->next=NULL;
            cell[hash]=p;
        }
        else
        {
            p->next=cell[hash];
            cell[hash]=p;
        }
    }
    //send_callback("build Table ended");
}

void SPHSystem::comp_dens_pres()
{
    //send_callback("dens started");
    Particle *p;
    Particle *np;
    
    int3 cell_pos;
    int3 near_pos;
    uint hash;
    
    float3 rel_pos;
    float r2;
    
    for(uint i=0; i<num_particle; i++)
    {
        p=&(mem[i]);
        cell_pos=calc_cell_pos(p->pos);
        
        p->dens=0.0f;
        p->pres=0.0f;
        
        for(int x=-1; x<=1; x++)
        {
            for(int y=-1; y<=1; y++)
            {
                for(int z=-1; z<=1; z++)
                {
                    near_pos.x=cell_pos.x+x;
                    near_pos.y=cell_pos.y+y;
                    near_pos.z=cell_pos.z+z;
                    hash=calc_cell_hash(near_pos);
                    
                    if(hash == 0xffffffff)
                    {
                        continue;
                    }
                    
                    np=cell[hash];
                    while(np != NULL)
                    {
                        rel_pos.x=np->pos.x-p->pos.x;
                        rel_pos.y=np->pos.y-p->pos.y;
                        rel_pos.z=np->pos.z-p->pos.z;
                        r2=rel_pos.x*rel_pos.x+rel_pos.y*rel_pos.y+rel_pos.z*rel_pos.z;
                        
                        if(r2<INF || r2>=kernel_2)
                        {
                            np=np->next;
                            continue;
                        }
                        
                        p->dens=p->dens + mass * poly6_value * pow(kernel_2-r2, 3);
                        
                        np=np->next;
                    }
                }
            }
        }
        
        p->dens=p->dens+self_dens;
        p->pres=(pow(p->dens / rest_density, 7) - 1) *gas_constant;
    }
    //send_callback("dens ended");
}

void SPHSystem::comp_force_adv()
{
    //send_callback("force started");
    Particle *p;
    Particle *np;
    
    int3 cell_pos;
    int3 near_pos;
    uint hash;
    
    float3 rel_pos;
    float3 rel_vel;
    
    float r2;
    float r;
    float kernel_r;
    float V;
    
    float pres_kernel;
    float visc_kernel;
    float temp_force;
    
    float3 grad_color;
    float lplc_color;
    
    for(uint i=0; i<num_particle; i++)
    {
        p=&(mem[i]);
        cell_pos=calc_cell_pos(p->pos);
        
        p->acc.x=0.0f;
        p->acc.y=0.0f;
        p->acc.z=0.0f;
        
        grad_color.x=0.0f;
        grad_color.y=0.0f;
        grad_color.z=0.0f;
        lplc_color=0.0f;
        
        for(int x=-1; x<=1; x++)
        {
            for(int y=-1; y<=1; y++)
            {
                for(int z=-1; z<=1; z++)
                {
                    near_pos.x=cell_pos.x+x;
                    near_pos.y=cell_pos.y+y;
                    near_pos.z=cell_pos.z+z;
                    hash=calc_cell_hash(near_pos);
                    
                    if(hash == 0xffffffff)
                    {
                        continue;
                    }
                    
                    np=cell[hash];
                    while(np != NULL)
                    {
                        rel_pos.x=p->pos.x-np->pos.x;
                        rel_pos.y=p->pos.y-np->pos.y;
                        rel_pos.z=p->pos.z-np->pos.z;
                        r2=rel_pos.x*rel_pos.x+rel_pos.y*rel_pos.y+rel_pos.z*rel_pos.z;
                        
                        if(r2 < kernel_2 && r2 > INF)
                        {
                            r=sqrt(r2);
                            V=mass/np->dens/2;
                            kernel_r=kernel-r;
                            
                            pres_kernel=spiky_value * kernel_r * kernel_r;
                            temp_force=V * (p->pres+np->pres) * pres_kernel;
                            p->acc.x=p->acc.x-rel_pos.x*temp_force/r;
                            p->acc.y=p->acc.y-rel_pos.y*temp_force/r;
                            p->acc.z=p->acc.z-rel_pos.z*temp_force/r;
                            
                            rel_vel.x=np->ev.x-p->ev.x;
                            rel_vel.y=np->ev.y-p->ev.y;
                            rel_vel.z=np->ev.z-p->ev.z;
                            
                            visc_kernel=visco_value*(kernel-r);
                            temp_force=V * viscosity * visc_kernel;
                            p->acc.x=p->acc.x + rel_vel.x*temp_force; 
                            p->acc.y=p->acc.y + rel_vel.y*temp_force; 
                            p->acc.z=p->acc.z + rel_vel.z*temp_force; 
                            
                            //float temp=(-1) * grad_poly6 * V * pow(kernel_2-r2, 2);
                            //grad_color.x += temp * rel_pos.x;
                            //grad_color.y += temp * rel_pos.y;
                            //grad_color.z += temp * rel_pos.z;
                            //lplc_color += lplc_poly6 * V * (kernel_2-r2) * (r2-3/4*(kernel_2-r2));
                        }
                        
                        np=np->next;
                    }
                }
            }
        }
        
        lplc_color+=self_lplc_color/p->dens;
        p->surf_norm=sqrt(grad_color.x*grad_color.x+grad_color.y*grad_color.y+grad_color.z*grad_color.z);
        
        if(p->surf_norm > surf_norm)
        {
            p->acc.x+=surf_coe * lplc_color * grad_color.x / p->surf_norm;
            p->acc.y+=surf_coe * lplc_color * grad_color.y / p->surf_norm;
            p->acc.z+=surf_coe * lplc_color * grad_color.z / p->surf_norm;
        }
    }
    //send_callback("force ended");
}

void SPHSystem::advection()
{
    //send_callback("advection started");
    Particle *p;
    for(uint i=0; i<num_particle; i++)
    {
        p=&(mem[i]);
        
        p->vel.x=p->vel.x+p->acc.x*time_step/p->dens+gravity.x*time_step;
        p->vel.y=p->vel.y+p->acc.y*time_step/p->dens+gravity.y*time_step;
        p->vel.z=p->vel.z+p->acc.z*time_step/p->dens+gravity.z*time_step;
        
        p->pos.x=p->pos.x+p->vel.x*time_step;
        p->pos.y=p->pos.y+p->vel.y*time_step;
        p->pos.z=p->pos.z+p->vel.z*time_step;
        
        if(p->pos.x >= world_size.x-BOUNDARY)
        {
            p->vel.x=p->vel.x*wall_damping;
            p->pos.x=world_size.x-BOUNDARY;
        }
        
        if(p->pos.x < 0.0f)
        {
            p->vel.x=p->vel.x*wall_damping;
            p->pos.x=0.0f;
        }
        
        if(p->pos.y >= world_size.y-BOUNDARY)
        {
            p->vel.y=p->vel.y*wall_damping;
            p->pos.y=world_size.y-BOUNDARY;
        }
        
        if(p->pos.y < 0.0f)
        {
            p->vel.y=p->vel.y*wall_damping;
            p->pos.y=0.0f;
        }
        
        if(p->pos.z >= world_size.z-BOUNDARY)
        {
            p->vel.z=p->vel.z*wall_damping;
            p->pos.z=world_size.z-BOUNDARY;
        }
        
        if(p->pos.z < 0.0f)
        {
            p->vel.z=p->vel.z*wall_damping;
            p->pos.z=0.0f;
        }
        
        p->ev.x=(p->ev.x+p->vel.x)/2;
        p->ev.y=(p->ev.y+p->vel.y)/2;
        p->ev.z=(p->ev.z+p->vel.z)/2;
    }
    //send_callback("advection ended");
}

inline int3 SPHSystem::calc_cell_pos(float3 p)
{
	int3 cell_pos;
	cell_pos.x = int(floor((p.x) / cell_size));
	cell_pos.y = int(floor((p.y) / cell_size));
	cell_pos.z = int(floor((p.z) / cell_size));

    return cell_pos;
}

inline uint SPHSystem::calc_cell_hash(int3 cell_pos)
{
	if(cell_pos.x<0 || cell_pos.x>=(int)grid_size.x || cell_pos.y<0 || cell_pos.y>=(int)grid_size.y || cell_pos.z<0 || cell_pos.z>=(int)grid_size.z)
	{
		return (uint)0xffffffff;
	}

	cell_pos.x = cell_pos.x & (grid_size.x-1);  
    cell_pos.y = cell_pos.y & (grid_size.y-1);  
	cell_pos.z = cell_pos.z & (grid_size.z-1);  

	return ((uint)(cell_pos.z))*grid_size.y*grid_size.x + ((uint)(cell_pos.y))*grid_size.x + (uint)(cell_pos.x);
}

std::string SPHSystem::float_conversion(float input)
{
    std::ostringstream buff;
    buff<<input;
    return buff.str();
}