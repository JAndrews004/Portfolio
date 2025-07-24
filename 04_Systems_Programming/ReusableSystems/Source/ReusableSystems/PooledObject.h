// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "PooledObject.generated.h"
DECLARE_DYNAMIC_MULTICAST_DELEGATE_OneParam(FOnPooledObjectDespawn, UPooledObject*, PoolActor);

UCLASS(Blueprintable, ClassGroup = (Custom), meta = (BlueprintSpawnableComponent))
class REUSABLESYSTEMS_API UPooledObject : public UActorComponent
{
	GENERATED_BODY()

public:	
	// Sets default values for this component's properties
	UPooledObject();

	FOnPooledObjectDespawn OnPooledObjectDespawn;

	UFUNCTION(BlueprintCallable, Category = "Pooled Object")
	void DeactivateObject();


	void SetActiveVariable(bool IsActive);
	void SetLifeSpan(float LifeTime);
	void SetPoolIndex(int index);

	bool IsActive();
	int GetPoolIndex();

	UFUNCTION(BlueprintImplementableEvent, Category = "MyEvents")
	void BP_EVENT_Activated();
	UFUNCTION(BlueprintImplementableEvent, Category = "MyEvents")
	void BP_EVENT_Deactivated();

protected:
	// Called when the game starts
	

	bool Active;
	float LifeSpan = 0.0f;
	int PoolIndex;

	FTimerHandle LifeSpanTimer;

		
};
