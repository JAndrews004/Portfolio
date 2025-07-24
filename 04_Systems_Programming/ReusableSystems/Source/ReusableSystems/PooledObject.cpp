// Fill out your copyright notice in the Description page of Project Settings.


#include "PooledObject.h"
#include "Bullet.h"

// Sets default values for this component's properties
UPooledObject::UPooledObject()
{
	// Set this component to be initialized when the game starts, and to be ticked every frame.  You can turn these features
	// off to improve performance if you don't need them.
	PrimaryComponentTick.bCanEverTick = true;
	
	// ...
}

void UPooledObject::DeactivateObject()
{
	SetActiveVariable(false);
	GetOwner()->GetWorldTimerManager().ClearAllTimersForObject(this);
	OnPooledObjectDespawn.Broadcast(this);
}

void UPooledObject::SetActiveVariable(bool IsActive)
{
	Active = IsActive;
	GetOwner()->SetActorHiddenInGame(!IsActive);
	GetOwner()->SetActorEnableCollision(IsActive);
	GetOwner()->SetActorTickEnabled(IsActive);
	GetOwner()->GetWorldTimerManager().SetTimer(LifeSpanTimer, this, &UPooledObject::DeactivateObject, LifeSpan, false);
}

void UPooledObject::SetLifeSpan(float LifeTime)
{
	LifeSpan = LifeTime;
}

void UPooledObject::SetPoolIndex(int index)
{
	PoolIndex = index;
}

bool UPooledObject::IsActive()
{
	return Active;
}

int UPooledObject::GetPoolIndex()
{
	return PoolIndex;
}

