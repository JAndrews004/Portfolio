// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "Lava.generated.h"

UCLASS()
class INTERACTIVEUE_API ALava : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	ALava();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

	UPROPERTY(EditAnywhere)
	float Damage = 5.0f;

	UPROPERTY(EditAnywhere)
	float DamageInterval = 0.5f;

	TMap<AActor*, FTimerHandle> DamageTimers;

	UFUNCTION()
	void OnOverlapBegin(UPrimitiveComponent* OverlappedComp, AActor* OtherActor, UPrimitiveComponent* OtherComp, int32 OtherBodyIndex,bool bFromSweep, const FHitResult& SweepResult);

	UFUNCTION()
	void OnOverlapEnd(UPrimitiveComponent* OverlappedComp, AActor* OtherActor,
		UPrimitiveComponent* OtherComp, int32 OtherBodyIndex);

	UPROPERTY(VisibleAnywhere)
	class UBoxComponent* CollisionBox;

	void ApplyDamage(AActor* Actor);


public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

};
