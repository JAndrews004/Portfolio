// Fill out your copyright notice in the Description page of Project Settings.

#include "GenericPoolManager.h"

// Sets default values for this component's properties
UGenericPoolManager::UGenericPoolManager()
{
	
}

// Called when the game starts
void UGenericPoolManager::BeginPlay()
{
	Super::BeginPlay();

	if (PooledObjectSubclass != nullptr)
	{
		UWorld* const World = GetWorld();

		if (World != nullptr) 
		{
			for (int i = 0; i < PoolSize; i++)
			{
				AActor* PoolableActor = World->SpawnActor<AActor>(PooledObjectSubclass, FVector().ZeroVector, FRotator().ZeroRotator);

				UPooledObject* PooledComp = PoolableActor->FindComponentByClass<UPooledObject>();

				if (PooledComp != nullptr)
				{
					PooledComp->SetActiveVariable(false);
					PooledComp->SetPoolIndex(i);
					PooledComp->OnPooledObjectDespawn.AddDynamic(this, &UGenericPoolManager::OnPooledObjectDespawn);
					ObjectPool.Add(PooledComp);
				}
			}
		}
	}
}

UPooledObject* UGenericPoolManager::SpawnPooledObject()
{
	for (UPooledObject* PoolableActor : ObjectPool)
	{

		if (PoolableActor != nullptr && !PoolableActor->IsActive()) 
		{
			
			PoolableActor->SetLifeSpan(PooledObjectLifeSpan);
			PoolableActor->SetActiveVariable(true);
			SpawnedPoolIndexes.Add(PoolableActor->GetPoolIndex());

			return PoolableActor;
		}


	}

	if (SpawnedPoolIndexes.Num() > 0)
	{
		int PooledObjectIndex = SpawnedPoolIndexes[0];
		SpawnedPoolIndexes.Remove(PooledObjectIndex);
		UPooledObject* PoolableActor = ObjectPool[PooledObjectIndex];

		if (PoolableActor != nullptr)
		{
			PoolableActor->SetActiveVariable(false);

			
			PoolableActor->SetLifeSpan(PooledObjectLifeSpan);
			PoolableActor->SetActiveVariable(true);
			SpawnedPoolIndexes.Add(PoolableActor->GetPoolIndex());

			return PoolableActor;
		}
	}

	return nullptr;

}

void UGenericPoolManager::OnPooledObjectDespawn(UPooledObject* PoolActor)
{
	SpawnedPoolIndexes.Remove(PoolActor->GetPoolIndex());

}

