// Fill out your copyright notice in the Description page of Project Settings.


#include "Bullet.h"
#include "GameFramework/ProjectileMovementComponent.h"
#include <Components/SphereComponent.h>

// Sets default values
ABullet::ABullet()
{
    PrimaryActorTick.bCanEverTick = true;
    

    // Pooling component
    PoolComp = CreateDefaultSubobject<UPooledObject>(TEXT("PooledComponent"));

   
}

void ABullet::BeginPlay()
{
	
}

void ABullet::ActivateBullet()
{
   //bullet movement logic
}

void ABullet::DeactivateBullet()
{
    //bullet movement logic
}




