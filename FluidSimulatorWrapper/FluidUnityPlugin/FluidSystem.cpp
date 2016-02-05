//
//  FluidSystem.cpp
//  FluidUnityPlugin
//
//  Created by Chris Smith on 15/12/2015.
//  Copyright © 2015 Chris Smith. All rights reserved.
//

#include "FluidSystem.hpp"
#include <stdlib.h>
#include <math.h>

#include "sph_data.h"
#include "sph_system.h"

namespace FluidWrapper
{

	class Particle
	{
	public:
		uint id;
		float3 pos;
		float3 vel;
		float3 acc;
		float3 ev;

		float dens;
		float pres;

		float surf_norm;

		Particle *next;
	};

	SPHSystem::SPHSystem()
	{

	}

	extern "C" int ** EXPORT_API fillArray(int size) {
		int i = 0, j = 0;
		int ** array = (int**)calloc(size, sizeof(int*));
		for (i = 0; i < size; i++) {
			array[i] = (int*)calloc(size, sizeof(int));
			for (j = 0; j < size; j++) {
				array[i][j] = i * size + j;
			}
		}
		return array;

	}

	//2d Array for each
	extern "C" Particle ** EXPORT_API RunAnimation(Particle** data)
	{
		
	}





}






